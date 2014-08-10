{
    ready: function() {
        var self = this;
        setInterval(function() {
            self.AdminRows.Get(null, function() {
                // Should load databound values in UI
            }, function() {
                self.AdminError.$.show();
            });
        }, 1000);
    }
}