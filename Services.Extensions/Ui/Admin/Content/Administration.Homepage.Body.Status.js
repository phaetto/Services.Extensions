{
    ready: function() {
        var self = this;

        self.ParseAutoStarting = function() {
            $(".autostart").each(function(i, el) {
                var $el = $(el);
                var checked = $el.attr("data-isautostarting") === "True" ? true : false;
                $el.attr("checked", checked);
            });
        };

        self.Refresh = function() {
            self.AdminRows.Get(null, function() {
                // Should load databound values in UI
                self.ParseAutoStarting();
                setTimeout(self.Refresh, 1000);
            }, function() {
                self.AdminError.$().show();
            });
        };

        setTimeout(self.Refresh, 100);

        self.$().on("change", ".autostart", function() {
            self.AdminRows.Post({ serviceId: $(this).attr("data-id") }, function() {
                // It should refresh automatically
            }, function() {
                self.AdminError.$().show();
            });
        });
    }
}