﻿using Com.Danliris.Service.Sales.Lib.Utilities;
using Com.Danliris.Service.Sales.Lib.ViewModels.IntegrationViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.Danliris.Service.Sales.Lib.ViewModels.SalesInvoice
{
    public class SalesInvoiceViewModel : BaseViewModel, IValidatableObject
    {
        public string Code { get; set; }
        public long? AutoIncreament { get; set; }
        public string SalesInvoiceNo { get; set; }
        public string SalesInvoiceType { get; set; }
        public string SalesInvoiceCategory { get; set; }
        public DateTimeOffset? SalesInvoiceDate { get; set; }
        public DateTimeOffset? DueDate { get; set; }
        public string DeliveryOrderNo { get; set; }
        public BuyerViewModel Buyer { get; set; }
        public CurrencyViewModel Currency { get; set; }
        public string PaymentType { get; set; }
        public string VatType { get; set; }
        public double? TotalPayment { get; set; }
        public double? TotalPaid { get; set; }
        public bool? IsPaidOff { get; set; }
        public string Remark { get; set; }
        public string Sales { get; set; }
        public UnitViewModel Unit { get; set; }
        public string SalesType { get; set; }

        //Export
        public DateTimeOffset? SailingDate { get; set; }
        public string ShippedPer { get; set; }
        public string Color { get; set; }
        public string OrderNo { get; set; }
        public string Indent { get; set; }
        public string CartonNo { get; set; }
        public string WeightUom { get; set; }
        public string TotalUom { get; set; }
        public double? QuantityLength { get; set; }
        public double? GrossWeight { get; set; }
        public double? NetWeight { get; set; }
        public double? TotalMeas { get; set; }

        public ICollection<SalesInvoiceDetailViewModel> SalesInvoiceDetails { get; set; }


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(SalesInvoiceType) || SalesInvoiceType == "")
                yield return new ValidationResult("Kode Faktur Penjualan harus diisi", new List<string> { "SalesInvoiceType" });

            if (!SalesInvoiceDate.HasValue || SalesInvoiceDate.Value > DateTimeOffset.Now)
                yield return new ValidationResult("Tgl Faktur Penjualan harus diisi & lebih kecil atau sama dengan hari ini", new List<string> { "SalesInvoiceDate" });

            if (Buyer == null || string.IsNullOrWhiteSpace(Buyer.Name))
                yield return new ValidationResult("Buyer harus diisi", new List<string> { "BuyerName" });

            if (Buyer == null || string.IsNullOrWhiteSpace(Buyer.NPWP))
                yield return new ValidationResult("NPWP Buyer harus diisi", new List<string> { "BuyerNPWP" });

            if (Buyer == null || string.IsNullOrWhiteSpace(Buyer.NIK))
                yield return new ValidationResult("NIK Buyer harus diisi", new List<string> { "BuyerNIK" });

            if (Currency == null || string.IsNullOrWhiteSpace(Currency.Code))
                yield return new ValidationResult("Kurs harus diisi", new List<string> { "CurrencyCode" });

            if (string.IsNullOrWhiteSpace(PaymentType) || PaymentType == "")
                yield return new ValidationResult("Pembayaran dalam satuan harus diisi", new List<string> { "PaymentType" });

            if (!DueDate.HasValue || Id == 0 && DueDate.Value < DateTimeOffset.Now.AddDays(-1))
                yield return new ValidationResult("Tanggal jatuh tempo kosong, Tempo belum diisi", new List<string> { "DueDate" });

            if (string.IsNullOrWhiteSpace(VatType) || VatType == "")
                yield return new ValidationResult("Jenis PPN harus diisi", new List<string> { "VatType" });

            if (!TotalPayment.HasValue || TotalPayment <= 0)
                yield return new ValidationResult("Total termasuk PPN kosong", new List<string> { "TotalPayment" });

            if (TotalPaid < 0)
                yield return new ValidationResult("Total Paid harus lebih besar atau sama dengan 0", new List<string> { "TotalPayment" });

            if (string.IsNullOrEmpty(Sales))
                yield return new ValidationResult("Sales Harus Diisi", new List<string> { "Sales" });

            if (Unit == null || string.IsNullOrWhiteSpace(Unit.Name))
                yield return new ValidationResult("Unit Harus Diisi", new List<string> { "Unit" });

            if (string.IsNullOrWhiteSpace(SalesType) || SalesType == "")
                yield return new ValidationResult("Tipe Penjualan harus dipilih", new List<string> { "SalesType" });

            //Export

            if (SalesType == "Ekspor")
            {
                if (!SailingDate.HasValue)
                    yield return new ValidationResult("Tgl Sailing harus diisi & lebih kecil atau sama dengan hari ini", new List<string> { "SailingDate" });

                if (string.IsNullOrEmpty(ShippedPer))
                    yield return new ValidationResult("Shipped Per Harus Diisi", new List<string> { "ShippedPer" });

                if (string.IsNullOrEmpty(Color))
                    yield return new ValidationResult("Warna Harus Diisi", new List<string> { "Color" });

                if (string.IsNullOrEmpty(OrderNo))
                    yield return new ValidationResult("No. Order Harus Diisi", new List<string> { "OrderNo" });

                if (string.IsNullOrEmpty(Indent))
                    yield return new ValidationResult("Indent Harus Diisi", new List<string> { "Indent" });

                if (string.IsNullOrEmpty(CartonNo))
                    yield return new ValidationResult("No.Karton Harus Diisi", new List<string> { "CartonNo" });

                if (WeightUom == "")
                    yield return new ValidationResult("Satuan berat harus dipilih", new List<string> { "WeightUom" });

                if (TotalUom == "")
                    yield return new ValidationResult("Satuan total harus dipilih", new List<string> { "TotalUom" });

                if (!QuantityLength.HasValue || QuantityLength <= 0)
                    yield return new ValidationResult("Total Panjang kosong", new List<string> { "QuantityLength" });

                if (!GrossWeight.HasValue || GrossWeight <= 0)
                    yield return new ValidationResult("Berat kotor kosong", new List<string> { "GrossWeight" });

                if (!NetWeight.HasValue || NetWeight <= 0)
                    yield return new ValidationResult("Berat bersih kosong", new List<string> { "NetWeight" });

                if (!TotalMeas.HasValue || TotalMeas <= 0)
                    yield return new ValidationResult("Ukuran total kosong", new List<string> { "TotalMeas" });
            }

            int Count = 0;
            string DetailErrors = "[";

            if (SalesInvoiceDetails != null && SalesInvoiceDetails.Count > 0)
            {
                foreach (var detail in SalesInvoiceDetails)
                {
                    int ErrorCount = 0;
                    DetailErrors += "{";

                    if (SalesInvoiceCategory == "DYEINGPRINTING" && (!detail.ShippingOutId.HasValue || string.IsNullOrWhiteSpace(detail.BonNo)))
                    {
                        Count++;
                        ErrorCount++;
                        DetailErrors += "BonNo : 'No. Bon Pengiriman kosong / tidak ditemukan',";
                    }

                    if (SalesInvoiceCategory != "DYEINGPRINTING" && string.IsNullOrWhiteSpace(detail.BonNo))
                    {
                        Count++;
                        ErrorCount++;
                        DetailErrors += "BonNo : 'No. Bon Pengiriman kosong',";
                    }

                    if (SalesInvoiceCategory == "DYEINGPRINTING")
                    {
                        var duplicate = SalesInvoiceDetails.Where(w => w.ShippingOutId.Equals(detail.ShippingOutId.GetValueOrDefault()) && w.BonNo.Equals(detail.BonNo)).ToList();

                        if (duplicate.Count > 1)
                        {
                            Count++;
                            DetailErrors += "BonNo : 'No. Bon Pengiriman duplikat',";
                        }
                    }
                    else
                    {
                        var duplicate = SalesInvoiceDetails.Where(w => w.BonNo != null && detail.BonNo != null && w.BonNo.Equals(detail.BonNo)).ToList();

                        if (duplicate.Count > 1)
                        {
                            Count++;
                            DetailErrors += "BonNo : 'No. Bon Pengiriman duplikat',";
                        }
                    }

                    if (ErrorCount == 0)
                    {
                        if (detail.SalesInvoiceItems == null || detail.SalesInvoiceItems.Count == 0)
                        {
                            Count++;
                            DetailErrors += "SalesInvoiceItem : 'Item Kosong',";
                        }
                        else
                        {
                            DetailErrors += "SalesInvoiceItems: [";

                            foreach (var item in detail.SalesInvoiceItems)
                            {
                                DetailErrors += "{";

                                if (string.IsNullOrWhiteSpace(item.ProductCode))
                                {
                                    Count++;
                                    DetailErrors += "ProductCode : 'Kode produk harus diisi',";
                                }

                                if (string.IsNullOrWhiteSpace(item.ProductName))
                                {
                                    Count++;
                                    DetailErrors += "ProductName : 'Nama produk harus diisi',";
                                }

                                if (!item.QuantityPacking.HasValue || item.QuantityPacking.Value <= 0)
                                {
                                    Count++;
                                    DetailErrors += "QuantityPacking : 'Jumlah Packing harus diisi dan lebih besar dari 0',";
                                }

                                if (!item.QuantityItem.HasValue || item.QuantityItem.Value <= 0)
                                {
                                    Count++;
                                    DetailErrors += "QuantityItem : 'Jumlah Item harus diisi dan lebih besar dari 0',";
                                }

                                if (string.IsNullOrWhiteSpace(item.PackingUom))
                                {
                                    Count++;
                                    DetailErrors += "PackingUom : 'Satuan Packing harus diisi',";
                                }

                                if (string.IsNullOrWhiteSpace(item.ItemUom))
                                {
                                    Count++;
                                    DetailErrors += "ItemUom : 'Satuan Item harus diisi',";
                                }

                                if (!item.Price.HasValue || item.Price.Value <= 0)
                                {
                                    Count++;
                                    DetailErrors += "Price : 'Harga barang harus diisi dan lebih besar dari 0',";
                                }

                                DetailErrors += "}, ";
                            }

                            DetailErrors += "], ";
                        }
                    }

                    DetailErrors += "}, ";
                }
            }
            else
            {
                yield return new ValidationResult("Detail harus diisi", new List<string> { "SalesInvoiceDetail" });
            }

            DetailErrors += "]";

            if (Count > 0)
                yield return new ValidationResult(DetailErrors, new List<string> { "SalesInvoiceDetails" });
        }
    }
}
