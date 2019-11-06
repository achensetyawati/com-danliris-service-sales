﻿using Com.Danliris.Service.Sales.Lib.BusinessLogic.Interface.Garment;
using Com.Danliris.Service.Sales.Lib.BusinessLogic.Logic.Garment;
using Com.Danliris.Service.Sales.Lib.Helpers;
using Com.Danliris.Service.Sales.Lib.Models.CostCalculationGarments;
using Com.Danliris.Service.Sales.Lib.Services;
using Com.Danliris.Service.Sales.Lib.ViewModels.Garment;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Com.Danliris.Service.Sales.Lib.BusinessLogic.Facades.Garment
{
    public class AvailableBudgetReportFacade : IAvailableBudgetReportFacade
    {
        private AvailableBudgetReportLogic logic;
        private IIdentityService identityService;

        public AvailableBudgetReportFacade(IServiceProvider serviceProvider)
        {
            logic = serviceProvider.GetService<AvailableBudgetReportLogic>();
            identityService = serviceProvider.GetService<IIdentityService>();
        }

        public Tuple<MemoryStream, string> GenerateExcel(string filter = "{}")
        {
            var Query = logic.GetQuery(filter);
            var data = GetData(Query.ToList());

            DataTable dataTable = new DataTable();
            dataTable.Columns.Add(new DataColumn() { ColumnName = "No", DataType = typeof(int) });
            dataTable.Columns.Add(new DataColumn() { ColumnName = "Tanggal Cost Calculation", DataType = typeof(string) });
            dataTable.Columns.Add(new DataColumn() { ColumnName = "Tanggal Kesiapan Budget\n(Validasi PPIC)", DataType = typeof(string) });
            dataTable.Columns.Add(new DataColumn() { ColumnName = "Tanggal Shipment", DataType = typeof(string) });
            dataTable.Columns.Add(new DataColumn() { ColumnName = "No RO", DataType = typeof(string) });
            dataTable.Columns.Add(new DataColumn() { ColumnName = "Artikel", DataType = typeof(string) });
            dataTable.Columns.Add(new DataColumn() { ColumnName = "+/-\nSiap - Shipment", DataType = typeof(int) });
            dataTable.Columns.Add(new DataColumn() { ColumnName = "Buyer", DataType = typeof(string) });
            dataTable.Columns.Add(new DataColumn() { ColumnName = "Quantity", DataType = typeof(double) });
            dataTable.Columns.Add(new DataColumn() { ColumnName = "Satuan", DataType = typeof(string) });

            List<(string, Enum, Enum)> mergeCells = new List<(string, Enum, Enum)>() { };

            if (data != null && data.Count > 0)
            {
                int i = 0;
                foreach (var d in data)
                {
                    dataTable.Rows.Add(++i, d.CostCalculationDate.ToString("dd MMMM yyyy", new CultureInfo("id-ID")), d.ApprovedPPICDate.ToString("dd MMMM yyyy", new CultureInfo("id-ID")), d.DeliveryDate.ToString("dd MMMM yyyy", new CultureInfo("id-ID")), d.RONo, d.Article, d.DateDiff, d.Buyer, d.Quantity, d.Uom);
                }
                dataTable.Rows.Add(null, null, null, null, null, null, null, null, null, null);
                dataTable.Rows.Add(null, null, null, null, null, null, null, null, null, null);

                var count = data.Count;
                var countOk = data.Count(d => d.DateDiff >= 35);
                var percentOk = ((decimal)countOk / count).ToString("P", new CultureInfo("id-ID"));
                var countNotOk = data.Count(d => d.DateDiff < 35);
                var percentNotOk = ((decimal)countNotOk / count).ToString("P", new CultureInfo("id-ID"));

                dataTable.Rows.Add(null, "KESIAPAN BUDGET", null, null, null, null, null, null, null, null);
                dataTable.Rows.Add(null, "Status OK", null, "Selisih Tgl Kesiapan Budget dengan Tgl Shipment >= 35 hari", null, null, null, null, null, null);
                dataTable.Rows.Add(null, "Persentase Status OK", null, $"{countOk}/{count} X 100% = {percentOk}", null, null, null, null, null, null);
                dataTable.Rows.Add(null, "Status NOT OK", null, "Selisih Tgl Kesiapan Budget dengan Tgl Shipment < 35 hari", null, null, null, null, null, null);
                dataTable.Rows.Add(null, "Persentase Status NOT OK", null, $"{countNotOk}/{count} X 100% = {percentNotOk}", null, null, null, null, null, null);

                i += 3;
                mergeCells.Add(($"B{++i}:J{i}", ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Bottom));
                foreach (var n in Enumerable.Range(0, 4))
                {
                    mergeCells.Add(($"B{++i}:C{i}", ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Bottom));
                    mergeCells.Add(($"D{i}:J{i}", ExcelHorizontalAlignment.Left, ExcelVerticalAlignment.Bottom));
                }
            }
            else
            {
                dataTable.Rows.Add(null, null, null, null, null, null, null, null, null, null);
            }

            var excel = Excel.CreateExcel(new List<(DataTable, string, List<(string, Enum, Enum)>)>() { (dataTable, "AvailableBudget", mergeCells) }, false);

            return Tuple.Create(excel, string.Concat("Laporan Kesiapan Budget", GetSuffixNameFromFilter(filter)));
        }

        private string GetSuffixNameFromFilter(string filterString)
        {
            Dictionary<string, object> filter = JsonConvert.DeserializeObject<Dictionary<string, object>>(filterString);

            return string.Join(null, filter.Where(w => w.Value != null).Select(s => string.Concat(" - ", s.Value is string ? s.Value : ((DateTime)s.Value).AddHours(identityService.TimezoneOffset).ToString("dd MMMM yyyy") )).ToArray());
        }

        public Tuple<List<AvailableBudgetReportViewModel>, int> Read(int page = 1, int size = 25, string filter = "{}")
        {
            var Query = logic.GetQuery(filter);
            var data = GetData(Query.ToList());

            return Tuple.Create(data, data.Count);
        }

        private List<AvailableBudgetReportViewModel> GetData(IEnumerable<CostCalculationGarment> CostCalculationGarments)
        {
            var data = CostCalculationGarments.Select(cc => new AvailableBudgetReportViewModel
            {
                CostCalculationDate = cc.CreatedUtc.AddHours(identityService.TimezoneOffset).Date,
                ApprovedPPICDate = cc.ApprovedPPICDate.ToOffset(TimeSpan.FromHours(identityService.TimezoneOffset)).Date,
                DeliveryDate = cc.DeliveryDate.ToOffset(TimeSpan.FromHours(identityService.TimezoneOffset)).Date,
                RONo = cc.RO_Number,
                Article = cc.Article,
                DateDiff = (cc.DeliveryDate.ToOffset(TimeSpan.FromHours(identityService.TimezoneOffset)).Date - cc.ApprovedPPICDate.ToOffset(TimeSpan.FromHours(identityService.TimezoneOffset)).Date).Days,
                Buyer = cc.BuyerBrandName,
                Quantity = cc.Quantity,
                Uom = cc.UOMUnit
            }).ToList();

            return data;
        }
    }
}
