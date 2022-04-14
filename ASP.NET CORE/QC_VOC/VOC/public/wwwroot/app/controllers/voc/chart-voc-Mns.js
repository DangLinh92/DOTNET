var chartVocMnsController = function () {
    this.initialize = function () {
        GetCustomer();
        //registerEvents();
    }

    function registerEvents() {

        $('#txtYear').on('dp.change', function (e) {
            console.log(e.date.format('YYYY'));

            var year = e.date.format('YYYY');
            var customer = $('#cboCustomer').val();
            var side = $('#cboSide').val();

            SearchDataChart(year, customer, side);
        });

        $('#cboCustomer').on('change', function () {
            console.log('2');

            var year = $('#txtYear').val();
            var customer = $('#cboCustomer').val();
            var side = $('#cboSide').val();

            SearchDataChart(year, customer, side);
        });

        $('#cboSide').on('change', function () {
            console.log('3');

            var year = $('#txtYear').val();
            var customer = $('#cboCustomer').val();
            var side = $('#cboSide').val();
            SearchDataChart(year, customer, side);
        });

        function SearchDataChart(year, customer, side) {
            $.ajax({
                type: "GET",
                url: "/Admin/Voc/Search",
                data: {
                    year: year,
                    customer: customer,
                    side: side
                },
                success: function (response) {
                    if (chartdataInit) {
                        chartdataInit = response.vOCSiteModelByTimeLsts;
                        chartdataTotalInit = response.totalVOCSitesView;
                        chartdataByDefectName = response.paretoDataDefectName;

                        $('#tagReceiveCount').text(response.vocProgessInfo.ReceiveCount);
                        $('#tagCloseCount').text(response.vocProgessInfo.CloseCount);
                        $('#tagProgressCount').text(response.vocProgessInfo.ProgressCount);
                        $('#vocByMonthTitle').text(side + '진성 VOC 건수');
                        $('#vocYearTitle').text(year.toString().substring(2, 4) + '년 VOC 접수 건수');

                        let dta = [];
                        for (item of response.vocProgessInfo.lstVocProgress) {

                            let dataItem = [];
                            dataItem.push(item.Received_site);
                            dataItem.push(item.PlaceOfOrigin);
                            dataItem.push(item.ReceivedDept);
                            dataItem.push(item.ReceivedDate);
                            dataItem.push(item.SPLReceivedDate);
                            dataItem.push(item.SPLReceivedDateWeek);
                            dataItem.push(item.Customer);
                            dataItem.push(item.SETModelCustomer);
                            dataItem.push(item.ProcessCustomer);
                            dataItem.push(item.ModelFullname);
                            dataItem.push(item.DefectNameCus);
                            dataItem.push(item.DefectRate);
                            dataItem.push(item.PartsClassification);
                            dataItem.push(item.PartsClassification2);
                            dataItem.push(item.ProdutionDateMarking);
                            dataItem.push(item.AnalysisResult);
                            dataItem.push(item.VOCCount);
                            dataItem.push(item.DefectCause);
                            dataItem.push(item.DefectClassification);
                            dataItem.push(item.CustomerResponse);
                            dataItem.push(item.Report_FinalApprover);
                            dataItem.push(item.Report_Sender);
                            dataItem.push(item.Rport_sentDate);
                            dataItem.push(item.VOCState);
                            dataItem.push(item.VOCFinishingDate);
                            dataItem.push(item.VOC_TAT);
                            dta.push(dataItem);
                        }

                        let dtaComplete = [];
                        for (item of response.vocProgessInfo.lstVocComplete) {

                            let dataItem = [];
                            dataItem.push(item.Received_site);
                            dataItem.push(item.PlaceOfOrigin);
                            dataItem.push(item.ReceivedDept);
                            dataItem.push(item.ReceivedDate);
                            dataItem.push(item.SPLReceivedDate);
                            dataItem.push(item.SPLReceivedDateWeek);
                            dataItem.push(item.Customer);
                            dataItem.push(item.SETModelCustomer);
                            dataItem.push(item.ProcessCustomer);
                            dataItem.push(item.ModelFullname);
                            dataItem.push(item.DefectNameCus);
                            dataItem.push(item.DefectRate);
                            dataItem.push(item.PartsClassification);
                            dataItem.push(item.PartsClassification2);
                            dataItem.push(item.ProdutionDateMarking);
                            dataItem.push(item.AnalysisResult);
                            dataItem.push(item.VOCCount);
                            dataItem.push(item.DefectCause);
                            dataItem.push(item.DefectClassification);
                            dataItem.push(item.CustomerResponse);
                            dataItem.push(item.Report_FinalApprover);
                            dataItem.push(item.Report_Sender);
                            dataItem.push(item.Rport_sentDate);
                            dataItem.push(item.VOCState);
                            dataItem.push(item.VOCFinishingDate);
                            dataItem.push(item.VOC_TAT);
                            dtaComplete.push(dataItem);
                        }

                        chartVoc.DrawChart();
                        apexVoc.DrawChart();

                        ReInitDataTable(dta, '#vocMstDataTable');
                        ReInitDataTable(dtaComplete, '#vocMstCompleteDataTable');

                    }
                },
                error: function (status) {
                    hrms.notify(status.responseText, 'error', 'alert', function () { });
                }
            });
        }
    }

    function GetCustomer() {
        $.ajax({
            url: '/Admin/Voc/GetCustomer',
            type: 'GET',
            success: function (response) {
                var render = "<option value=''>All</option>";
                $.each(response, function (i, item) {
                    render += "<option value='" + item.trim() + "'>" + item + "</option >"
                });
                $('#cboCustomer').html(render);
                $('#cboCustomer').val(selectCustomer);
                $('#cboCustomer').trigger('change');
            },
            error: function (status) {
                hrms.notify(status.responseText, 'error', 'alert', function () { });
            }
        });
    }

    function ReInitDataTable(datas, idTable) {

        var table = $(idTable);
        if (table) {
            table.DataTable().destroy();
        }

        var newTable = $(idTable).DataTable({
            data: datas,
            scrollY: 400,
            scrollX: true,
            scrollCollapse: true,
            paging: false,
            select: true,
            "searching": false,
            fixedColumns: {
                leftColumns: 5
            },
            columnDefs: [{
                render: function (data, type, full, meta) {
                    return "<div class='text-wrap width-100'>" + data + "</div>";
                },
                targets: '_all'
            }],
            "order": [[0, 'asc']]
        });
        newTable.columns.adjust().draw();
    }
}