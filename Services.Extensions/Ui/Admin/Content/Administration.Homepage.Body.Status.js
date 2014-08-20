{
    ready: function() {
        var self = this;

        self.Refresh = function() {
            self.AdminRows.Get(null, function() {
                // Should load databound values in UI
                setTimeout(self.Refresh, 1000);
            }, function() {
                self.AdminError.$().show();
            });
        };

        setTimeout(self.Refresh, 100);
    }
}