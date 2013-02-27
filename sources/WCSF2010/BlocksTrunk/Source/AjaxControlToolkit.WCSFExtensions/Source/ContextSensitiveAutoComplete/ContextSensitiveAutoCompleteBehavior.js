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

AjaxControlToolkit.WCSFExtensions.ContextSensitiveAutoCompleteBehavior = function(element) {
    /// <summary>
    /// This behavior can be attached to a textbox to enable auto-complete/auto-suggest scenarios.
    /// </summary>
    /// <param name="element" type="Sys.UI.DomElement" DomElement="true" mayBeNull="false">
    /// DOM Element the behavior is associated with
    /// </param>
    /// <returns />
    AjaxControlToolkit.WCSFExtensions.ContextSensitiveAutoCompleteBehavior.initializeBase(this, [element]);

        this._completionContextItems = null;
        this._contextualTickHandler = null;
}
AjaxControlToolkit.WCSFExtensions.ContextSensitiveAutoCompleteBehavior.prototype = {
    initialize: function() {
        /// <summary>
        /// Initializes the autocomplete behavior.
        /// </summary>
        /// <returns />
        this._contextualTickHandler = Function.createDelegate(this, this._onContextualTimerTick);

        AjaxControlToolkit.WCSFExtensions.ContextSensitiveAutoCompleteBehavior.callBaseMethod(this, 'initialize');

    },


    initializeTimer: function(timer) {
        /// <summary>
        /// Initializes the timer
        /// </summary>
        /// <param name="timer" type="Sys.UI.Timer" DomElement="false" mayBeNull="false" />
        /// <returns />
        timer.set_interval(this._completionInterval);
        timer.add_tick(this._contextualTickHandler);
    },

    dispose: function() {
        /// <summary>
        /// Disposes the autocomplete behavior
        /// </summary>
        /// <returns />

        this._contextualTickHandler = null;

        AjaxControlToolkit.AutoCompleteBehavior.callBaseMethod(this, 'dispose');
    },


    _onContextualTimerTick: function(sender, eventArgs) {
        /// <summary>
        /// Handler invoked when a timer tick occurs
        /// </summary>
        /// <param name="sender" type="Object" mayBeNull="true"/>
        /// <param name="eventArgs" type="Sys.EventArgs" mayBeNull="true" />
        /// <returns />

        if (this._servicePath && this._serviceMethod) {
            var text = this._currentCompletionWord();

            if (text.trim().length < this._minimumPrefixLength) {
                this._currentPrefix = null;
                this._update('', null, /* cacheResults */ false);
                return;
            }
            // there is new content in the textbox or the textbox is empty but the min prefix length is 0
           if ((this._currentPrefix !== text) || ((text == "") && (this._minimumPrefixLength == 0))) {
                this._currentPrefix = text;
                // Raise the populating event and optionally cancel the web service invocation
                var eventArgs = new Sys.CancelEventArgs();
                this.raisePopulating(eventArgs);
                if (eventArgs.get_cancel()) {
                    return;
                }

                // Create the service parameters and optionally add the context parameter
                // (thereby determining which method signature we're expecting...)
                var parentValues = new Array();
                var parentDictionary = new Object();
                var i;
                for(contextKey in this._completionContextItems){
                    var aParent = $get(this._completionContextItems[contextKey]);
                    parentDictionary[contextKey] = aParent.value;
                    parentValues.push(aParent.value);
                }

                var params = { prefixText : this._currentPrefix, count: this._completionSetCount, contextValues : parentDictionary};

                // Create context information for service call. Context is used as a key for caching purposes.
                var contextValues = parentValues;
                contextValues.push(this._currentPrefix);
                var context = contextValues.join("|");
                if ((context != "") && this._cache && this._cache[context]) {
                    this._update(text, this._cache[context], /* cacheResults */ false);
                    return;
                }

                if (this._useContextKey) {
                    params.contextKey = this._contextKey;
                }

                // Invoke the web service
                Sys.Net.WebServiceProxy.invoke(this.get_servicePath(), this.get_serviceMethod(), false, params,
                                            Function.createDelegate(this, this._onMethodContextualComplete),
                                            Function.createDelegate(this, this._onMethodFailed),
                                            context);
                $common.updateFormToRefreshATDeviceBuffer();
            }
        }
    },

    get_completionContextItems: function(){
        /// <value type="String">Comma separated ID of the parents elements. </value>
        return this._completionContextItems;
    },
    set_completionContextItems: function(value){
        this._completionContextItems = value;
    },
    _onMethodContextualComplete: function(result, context) {
        /// <summary>
        /// Handler invoked when the webservice call is completed.
        /// </summary>
        /// <param name="result" type="Object" DomElement="false" mayBeNull="true" />
        /// <param name="context" type="String" DomElement="false" mayBeNull="true" />
        /// <returns /> 
        if (this.get_enableCaching()) {
            if (!this._cache) {
                this._cache = {};
            }
            this._cache[context] = result;
        }
        var prefixText = context;
        var indexOfDelimiter = context.lastIndexOf("|");
        if (indexOfDelimiter >= 0) {
            prefixText = context.substring(indexOfDelimiter+1, context.length);
        }
        this._update(prefixText , result, /* cacheResults */ true);
    }

}
AjaxControlToolkit.WCSFExtensions.ContextSensitiveAutoCompleteBehavior.descriptor = {
    properties: [   {name: 'completionInterval', type: Number},
                    {name: 'completionList', isDomElement: true},
                    {name: 'completionListElementID', type: String},
                    {name: 'completionSetCount', type: Number},
                    {name: 'completionContextItems', type: Object},
                    {name: 'minimumPrefixLength', type: Number},
                    {name: 'serviceMethod', type: String},
                    {name: 'servicePath', type: String} ]
}
AjaxControlToolkit.WCSFExtensions.ContextSensitiveAutoCompleteBehavior.registerClass('AjaxControlToolkit.WCSFExtensions.ContextSensitiveAutoCompleteBehavior', AjaxControlToolkit.AutoCompleteBehavior);
