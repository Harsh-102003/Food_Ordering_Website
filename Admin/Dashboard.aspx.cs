using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace E_commerceWebsite.Admin
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                Session["breadCrum"] = "";
                if (Session["admin"] == null)
                {
                    Response.Redirect("../User/Login.aspx");
                }
                else
                {
                    //We created the obj of the (DashboardCount) class
                    //("CATEGORY", "PRODUCTS") this are the Action  name from which the data is fatched
                    DashboardCount dashboard = new DashboardCount();
                    Session["category"] = dashboard.Count("CATEGORY");
                    Session["product"] = dashboard.Count("PRODUCT");
                    Session["order"] = dashboard.Count("ORDER");
                    Session["delivered"] = dashboard.Count("DELIVERED");
                    Session["pending"] = dashboard.Count("PENDING");
                    Session["user"] = dashboard.Count("USER");
                    Session["soldAmount"] = dashboard.Count("SOLDAMOUNT");
                    Session["contact"] = dashboard.Count("CONTACT");
                }
            }
        }
    }
}