using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using System.Web.UI.WebControls.Adapters;

namespace E_commerceWebsite.User
{
    public partial class Profile : System.Web.UI.Page
    {
        SqlConnection con;
        SqlCommand cmd;
        SqlDataAdapter sdt;
        DataTable dt;

        protected void Page_Load(object sender, EventArgs e)
        {
            //checking the user is Login or not
            //(!IsPostBack) means the page is loading for first time
            if (!IsPostBack)
            {
                if (Session["userId"] == null)
                {
                        Response.Redirect("Login.aspx");
                }
                else
                {
                    getUserDetails();
                    getPurchaseHistory();
                }
            }
        }

        void getUserDetails()
        {
            //Call this method in page_load
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("User_Crud", con);
            cmd.Parameters.AddWithValue("@Action", "SELECT4PROFILE");
            cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
            cmd.CommandType = CommandType.StoredProcedure;
            sdt = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sdt.Fill(dt);
            rUserProfile.DataSource = dt;
            rUserProfile.DataBind();

            if (dt.Rows.Count == 1)
            {
                Session["name"] = dt.Rows[0]["Name"].ToString();
                Session["email"] = dt.Rows[0]["Email"].ToString();
                Session["imageUrl"] = dt.Rows[0]["ImageUrl"].ToString();
                Session["createdDate"] = dt.Rows[0]["CreatedDate"].ToString();
            }
        }

        void getPurchaseHistory()
        {
            int sr = 1;
            con = new SqlConnection(Connection.GetConnectionString());
            cmd = new SqlCommand("Invoice", con);
            cmd.Parameters.AddWithValue("@Action", "ODRHISTORY");
            cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
            cmd.CommandType = CommandType.StoredProcedure;
            sdt = new SqlDataAdapter(cmd);
            dt = new DataTable();
            sdt.Fill(dt);
            //adding column for SrNo
            dt.Columns.Add("SrNo", typeof(Int32));
            if(dt.Rows.Count> 0)
            {
                foreach(DataRow datarow in dt.Rows)
                {
                    datarow["SrNo"] = sr;
                    sr++;
                }
            }

            if (dt.Rows.Count == 0)
            {
                rPurchaseHistory.FooterTemplate = null;
                //we're calling the constructor of that custom template class 
                rPurchaseHistory.FooterTemplate = new CustomTemplate(ListItemType.Footer);
            }
            rPurchaseHistory.DataSource = dt;
            rPurchaseHistory.DataBind();
        }


        protected void rPurchaseHistory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if(e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                double grandTotal = 0;
                HiddenField paymentId = e.Item.FindControl("hdnPaymentId") as HiddenField;
                Repeater repOrders = e.Item.FindControl("rOrders") as Repeater;

                con = new SqlConnection(Connection.GetConnectionString());
                cmd = new SqlCommand("Invoice", con);
                cmd.Parameters.AddWithValue("@Action", "INVOICBYID");
                cmd.Parameters.AddWithValue("@PaymentId", Convert.ToInt32(paymentId.Value));
                cmd.Parameters.AddWithValue("@UserId", Session["userId"]);
                cmd.CommandType = CommandType.StoredProcedure;
                sdt = new SqlDataAdapter(cmd);
                dt = new DataTable();
                sdt.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow datarow in dt.Rows)
                    {
                        grandTotal += Convert.ToDouble(datarow["TotalPrice"]);
                    }
                }

                DataRow dr = dt.NewRow();
                dr["TotalPrice"] = grandTotal;
                dt.Rows.Add(dr);
                repOrders.DataSource = dt;
                repOrders.DataBind();
            }
            
        }

        // Custom template class to add controls to the repeater's header, item and footer sections.
        private sealed class CustomTemplate : ITemplate
        {

            private ListItemType ListItemType
            {
                get; set;
            }

            public CustomTemplate(ListItemType type)
            {
                ListItemType = type;
            }

            public void InstantiateIn(Control container)
            {

                if (ListItemType == ListItemType.Footer)
                {
                    var footer = new LiteralControl("<tr><td><b>Hungry! Why Not Order Food For You.</b><a href='Menu.aspx' class='badge badge-info ml-2'>Click To Order</a></td></tr></tbody></table>");
                    container.Controls.Add(footer);
                }
            }
        }

    }
}