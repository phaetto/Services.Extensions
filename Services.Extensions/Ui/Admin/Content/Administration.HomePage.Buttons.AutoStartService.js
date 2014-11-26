{
    databound: function() {
        var checked = this.$().attr("data-isautostarting") === "true" ? true : false;
        this.$().attr("checked", checked);
    },

    ready: function() {
        var checked = this.$().attr("data-isautostarting") === "true" ? true : false;
        this.$().attr("checked", checked);
    },

    change: function() {
        this.PostValues();
    }
}