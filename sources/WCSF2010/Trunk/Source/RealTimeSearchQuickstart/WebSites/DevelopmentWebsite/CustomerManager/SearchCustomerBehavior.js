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

var latestCustomersQuery = '';
var latestCityQuery = '';
var latestStateQuery = '';
var latestPostalCodeQuery = '';

function requestCustomersLoop() {
    var customersQuery = $get(companyNameTextBoxId).value;
    var cityQuery = $get(cityTextBoxId).value;
    var stateQuery = $get(stateDropDownId).value;
    var postalCodeQuery = $get(postalCodeTextBoxId).value;

    var customerNameValidator = $get(companyNameValidatorId);
    var cityValidator = $get(cityValidatorId);
    var postalCodeValidator = $get(postalCodeValidatorId);

    if ( customersQuery != latestCustomersQuery ||
        cityQuery != latestCityQuery ||
        stateQuery != latestStateQuery ||
        postalCodeQuery != latestPostalCodeQuery ) {

    ValidatorValidate( customerNameValidator , null , null );
    ValidatorValidate( cityValidator , null , null );
    ValidatorValidate( postalCodeValidator , null , null );

    if ( customerNameValidator.isvalid &&
        cityValidator.isvalid &&
        postalCodeValidator.isvalid ) {
        
        // eval(callbackMethod);  
        }

    latestCustomersQuery = customersQuery;
    latestCityQuery = cityQuery;
    latestStateQuery = stateQuery;      
    latestPostalCodeQuery = postalCodeQuery;
    }
 setTimeout('requestCustomersLoop();', 1800);
}