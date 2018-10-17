(function () {
    $(".data_table").on('click', 'th.sortable', function () {

        var propertyName = $(this).data('sort-property');
        var sortType = $(this).data('sort-type');

        $.ajax({
            "method": "GET",
            "url": "Home/DataTable",
            "data": { PropertyName: propertyName, SortType: sortType },

            "success": function (data) {
                $('.data_table').html(data);

                $('tr [data-sort-property=' + propertyName + ']').data('sort-type', sortType == 0 ? 1 : 0);

            }
        });
    });

    $('#searchForm').change(function () {
        $.ajax({
            "method": "POST",
            "url": $(this).prop("action"),
            "data": $(this).serialize(),            

            "success": function (data) {
                $('.data_table').html(data);
            }
        });
    });
})();