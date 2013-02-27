// README
//
// There are two steps to adding a property:
//
// 1. Create a member variable to store your property
// 2. Add the get_ and set_ accessors for your property.
//
// Remember that both are case sensitive!
//
Type.registerNamespace('RealTimeSearch');

RealTimeSearch.RealTimeSearchBehavior = function(element) {

    RealTimeSearch.RealTimeSearchBehavior.initializeBase(this, [element]);

    // TODO : (Step 1) Add your property variables here
    //
    this._controlsToMonitor = null;
    this._interval = null;
    
    this._lastQueryValues = null;
}

RealTimeSearch.RealTimeSearchBehavior.prototype = {

    initialize : function() {
        RealTimeSearch.RealTimeSearchBehavior.callBaseMethod(this, 'initialize');
        // TODO: add your initalization code here
        
        // register timer            
        setTimeout(Function.createDelegate(this, this.requestLoop), this.get_interval());
    },

    dispose : function() {
        // TODO: add your cleanup code here
        RealTimeSearch.RealTimeSearchBehavior.callBaseMethod(this, 'dispose');
    },

    // TODO: (Step 2) Add your property accessors here
    //
    get_controlsToMonitor: function(){
        return this._controlsToMonitor;
    },
    set_controlsToMonitor: function(value){
        this._controlsToMonitor = value;
    },
    get_interval: function(){
        return this._interval;
    },
    set_interval: function(value){
        this._interval = value;
    },
    requestLoop: function() {
      this.doRequest();
      setTimeout(Function.createDelegate(this, this.requestLoop), this.get_interval());
    },
    doRequest: function() {
      var queryValues = this._getValues();
      var indexOfDiff = this._getIndexOfDiff( queryValues , this._lastQueryValues );
      if ( indexOfDiff != -1 ) {

        // force all validations and execute if all passed
        if (this._validate())
        {
            // if there is a diff, execute callback
            eval(this._controlsToMonitor[indexOfDiff].Callback);
        }

        // store query arguments
        this._lastQueryValues = queryValues;
      }
    },
    
    _validate: function() {
      var globalIsValid=true;
      for (ctm in this._controlsToMonitor)
      {
        var validators = this._controlsToMonitor[ctm].Validators;
        for (validatorIndex in validators)
        {
            var validator = $get(validators[validatorIndex])
            ValidatorValidate( validator , null , null );
            globalIsValid = (validator.isvalid && globalIsValid);
        }
      }
      return globalIsValid;
    },
    
    _getValues: function() {
      var result = new Object();
      for (var index = 0; index < this._controlsToMonitor.length; index++) {
        var ctm = this._controlsToMonitor[index];
        result[ctm.TargetId] = $get(ctm.TargetId).value;
      }
      return result;
    },
    _getIndexOfDiff: function( val1 , val2 ) {
      if ( this._controlsToMonitor.length == 0 )
        return -1;
      if ( val1 == null && val2 == null )
        return -1;
      if ( val1 == null || val2 == null )
        return 0;
      // assert( val1 != null && val2 != null );
      for (var index = 0; index < this._controlsToMonitor.length; index++) {
        var ctm = this._controlsToMonitor[index];
        if ( (ctm.TargetId in val1) != (ctm.TargetId in val2) ) {
          return index;
        } else if (ctm.TargetId in val1) {
          // assert( ctm.TargetId in val1 );
          // assert( ctm.TargetId in val2 );
          if ( val1[ctm.TargetId] != val2[ctm.TargetId] )
            return index;
        }
      }
      return -1;
    }
}

RealTimeSearch.RealTimeSearchBehavior.descriptor = {
    properties: [   {name: 'controlsToMonitor', type: Object},
                    {name: 'interval', type: String} ]                    
}

RealTimeSearch.RealTimeSearchBehavior.registerClass('RealTimeSearch.RealTimeSearchBehavior', Sys.UI.Control); 
