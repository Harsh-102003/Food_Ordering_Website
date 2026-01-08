//'use strict';
(function($) {
    /*--------------
     Quantity change
    ---------------*/
    var proQty = $('.pro-qty');
    proQty.prepend('<span class="dec qtybtn" >-</span>');
    proQty.append('<span class="inc qtybtn">+</spans');
    proQty.on('click', '.qtybtn', function() {
        var $button = $(this);
        var oldValue = $button.parent().find('input').val();
        if ($button.hasClass('inc')) {
            //var newVal = parseFloat(oldValue) + 1;
            if (oldValue >= 10) {
                var newVal = parseFloat(oldValue);
            } else {
                newVal = parseFloat(oldValue) + 1;
            } {
                // Don't allow decrementing below zero
                if (oldValue > 1) {
                    var newVal = parseFloat(oldValue) - 1;
                } else {
                    newVal = 1;
                }
            }
            $button.parent().find('input').val(newVal);
        }
    });

})(jQuery);
