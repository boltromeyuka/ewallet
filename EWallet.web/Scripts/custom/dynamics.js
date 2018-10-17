(function () {    
    google.charts.load('current', { 'packages': ['corechart'], 'language': 'ru' });

    $('#chartForm').change(function () {
        $('.loader').show();
        $.ajax({
            "method": "POST",
            "url": $(this).prop("action"),            
            "data": $(this).serialize(),
            "dataType": 'json',           

            "success": function (data) {
                AddChart(data, null);
                $('.loader').hide();
            }            
        });
    });

    $('form').trigger('change');
})();

function AddChart(jsonData, selector) {

    google.charts.setOnLoadCallback(drawChart);

    function drawChart() {        

        var dt = new google.visualization.DataTable();
        dt.addColumn('date', 'Дата');
        for (var i = 1; i < jsonData[0].length; i++) {
            dt.addColumn('number', jsonData[0][i]);
        }       
        

        $.each(jsonData, function (i, chart) {
            if (i == 0)
                return true;

            var row = [new Date(chart[0].replace(/(\d{2}).(\d{2}).(\d{4})/, "$2/$1/$3"))];

            for (var i = 1; i < chart.length; i++) {
                row.push(parseFloat(chart[i]));
            }
                dt.addRow(row);
            
        });

        var options = {
            legend: { position: 'bottom' },
            
            'height': 500
        };

        var chart = new google.visualization.LineChart(document.getElementById('chart'));

        chart.draw(dt, options);    
    }
};