{
    ready: function() {
        var self = this;

        self.Refresh = function() {
            self.AdminRows.GetValues({}, function() {
                self.AdminRows.Databind(JSON.parse(self.AdminRows.Data.runtimeApplicationData));
                setTimeout(self.Refresh, 1000);
            }, function() {
                self.AdminError.$().show();
                setTimeout(self.Refresh, 5000);
            });
        };

        setTimeout(self.Refresh, 100);
    }
}