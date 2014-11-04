{
    ready: function() {
        var self = this;

        self.ParseAutoStarting = function() {
            $(".autostart").each(function(i, el) {
                var $el = $(el);
                var checked = $el.attr("data-isautostarting") === "true" ? true : false;
                $el.attr("checked", checked);
            });
        };

        self.Refresh = function() {
            $.ajax({
                url: "/administration/data",
                type: 'get'
            }).done(function( data ) {
                self.AdminRows.Databind(data);
                self.ParseAutoStarting();
                setTimeout(self.Refresh, 1000);
            }).error(function() {
                self.AdminError.$().show();
            });
        };

        setTimeout(self.Refresh, 100);

        self.$().on("change", ".autostart", function() {
            $.ajax({
                url: "/administration/data",
                type: 'post',
                data: {
                    serviceId: encodeURIComponent($(this).attr("data-id"))
                }
            });
        });
    }
}