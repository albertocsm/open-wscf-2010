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

// README
//
// There are two steps to adding a property:
//
// 1. Create a member variable to store your property
// 2. Add the get_ and set_ accessors for your property.
//
// Remember that both are case sensitive!
//

Type.registerNamespace('CustomExtenders');

CustomExtenders.ClientScriptCallBehavior = function(element) {

    CustomExtenders.ClientScriptCallBehavior.initializeBase(this, [element]);

    // TODO : (Step 1) Add your property variables here
    //
    this._customScript = null;
    this._changeHandler = null;
    this._customScriptParameters = null;

}

CustomExtenders.ClientScriptCallBehavior.prototype = {

    initialize : function() {
        CustomExtenders.ClientScriptCallBehavior.callBaseMethod(this, 'initialize');
        var targetElement = this.get_element();

        this._changeHandler = Function.createDelegate(this, this._onTargetChange);
        $addHandler(targetElement, "change", this._changeHandler);

        // TODO: add your initalization code here
    },

    dispose : function() {
        // TODO: add your cleanup code here
        $removeHandler(this.get_element(), "change", this._changeHandler);
        this._changeHandler = null;
        CustomExtenders.ClientScriptCallBehavior.callBaseMethod(this, 'dispose');
    },

    // TODO: (Step 2) Add your property accessors here
    //
    get_customScript : function() {
        return this._customScript;
    },
    set_customScript : function(value) {
        this._customScript = value;
    },

    get_customScriptParameters: function(){
        return this._customScriptParameters;
    },
    set_customScriptParameters: function(value){
        this._customScriptParameters = value;
    },

    _onTargetChange : function() {

        eval(this._customScript + "(this.get_element(), '" + this._customScriptParameters.join("', '") + "')");

    }
}

CustomExtenders.ClientScriptCallBehavior.descriptor = {
    properties: [   {name: 'customScriptParameters', type: Object},
                    {name: 'customScript', type: String} ]
}

CustomExtenders.ClientScriptCallBehavior.registerClass('CustomExtenders.ClientScriptCallBehavior', AjaxControlToolkit.BehaviorBase);
