﻿{
    ready: function() {
        var _this = this;
        
        function drawVisualization() {
            // Create and populate the data table.
            var data = google.visualization.arrayToDataTable([
                ['Task', 'Hours per Day'],
                ['Work', 11],
                ['Eat', 2],
                ['Commute', 2],
                ['Watch TV', 2],
                ['Sleep', 7]
            ]);

            // Create and draw the visualization.
            new google.visualization.PieChart(_this.PerformancePieChart.Element).
                draw(data, { title: "So, how was your day?" });
        }
        
        $(function() {
            // google.load('visualization', '1', { packages: ['corechart'], callback: drawVisualization });
        });
    }
}