$(document).ready(function () {
    $('a.btn-remove').click(removeCartItem);
})

function removeCartItem(e) {
    var buttonClicked = e.currentTarget;
    var cartDetailsId = buttonClicked.getAttribute('data-id');
    var removeCartUrl = buttonClicked.getAttribute('data-requestUrl');
    $.ajax({
        url: removeCartUrl,
        type: 'GET',
        data: { cartDetailsId: cartDetailsId },
        success: function (res) {
            if (res.success) {
                $(buttonClicked).closest('.product').remove();
                UpdateCartTotal();
            }
        }
    });
}

function UpdateCartTotal() {
    var dropdownCart = document.getElementsByClassName('dropdown-cart-products')[0];
    var products = dropdownCart.getElementsByClassName('product');
    var total = 0;
    var count = 0;
    for (var i = 0; i < products.length; i++) {
        count++;
        var product = products[i];
        var priceElement = product.getElementsByClassName('cart-product-info')[0];
        var quantityElement = product.getElementsByClassName('cart-product-qty')[0];
        var price = parseFloat(priceElement.textContent.split('$')[1].trim());
        var quantity = parseInt(quantityElement.textContent);
        total = total + (price * quantity);
    }
    document.getElementsByClassName('cart-count')[0].innerText = count;
    total = Math.round(total * 100) / 100;
    document.getElementsByClassName('cart-total-price')[0].innerText = '$' + total.toFixed(2);

}