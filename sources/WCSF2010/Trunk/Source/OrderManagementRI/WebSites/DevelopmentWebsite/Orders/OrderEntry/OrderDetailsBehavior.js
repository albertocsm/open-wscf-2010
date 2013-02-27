//===============================================================================
// Microsoft patterns & practices
// Web Client Software Factory
//-------------------------------------------------------------------------------
// Copyright (C) Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//-------------------------------------------------------------------------------
// The example companies, organizations, products, domain names,
// e-mail addresses, logos, people, places, and events depicted
// herein are fictitious.  No association with any real company,
// organization, product, domain name, email address, logo, person,
// places, or events is intended or should be inferred.
//===============================================================================

function GetProduct(textBox, productNameLabelId, priceLabelId)
{
    var context = new Object();
    context.SkuTextBoxId = textBox.id;
    context.ProductNameLabelId = productNameLabelId;
    context.PriceLabelId = priceLabelId;
    WebApplication.Orders.ProductServiceProxy.GetProduct(textBox.value, OnGetProductSuccess, OnGetProductFailure, context);
}

function OnGetProductSuccess(result, context)
{
    if (result!=null)
    {
        $get(context.PriceLabelId).innerHTML = result.UnitPrice;
        $get(context.ProductNameLabelId).innerHTML = result.ProductName;
        $get(context.SkuTextBoxId).style.background = "white";
        $get(context.SkuTextBoxId).style.color = "black";
    } else {
        $get(context.PriceLabelId).innerHTML = "0";
        $get(context.ProductNameLabelId).innerHTML = "(Description not found)";
        $get(context.SkuTextBoxId).focus();
        $get(context.SkuTextBoxId).style.background = "red";
        $get(context.SkuTextBoxId).style.color = "white";
//        alert("There is no product with sku '" + $get(context.SkuTextBoxId).value + "'");
    }
}

function OnGetProductFailure(error, context)
{
    $get(context.SkuTextBoxId).focus();
    $get(context.SkuTextBoxId).style.background = "red";
    $get(context.SkuTextBoxId).style.color = "white";
    alert(error.get_message());
}

function CalculateTotal(textBox, priceLabelId, itemTotalLabelId, currencySymbol)
{
    var quantity = textBox.value;
    var price = $get(priceLabelId).innerHTML;
    var total = quantity * price;
    $get(itemTotalLabelId).innerHTML = currencySymbol + total.toString();
}
