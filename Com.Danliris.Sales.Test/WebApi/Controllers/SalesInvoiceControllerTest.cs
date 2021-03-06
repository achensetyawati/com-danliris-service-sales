﻿using AutoMapper;
using Com.Danliris.Sales.Test.WebApi.Utils;
using Com.Danliris.Service.Sales.Lib.AutoMapperProfiles.SalesInvoiceProfiles;
using Com.Danliris.Service.Sales.Lib.BusinessLogic.Interface.SalesInvoice;
using Com.Danliris.Service.Sales.Lib.Models.SalesInvoice;
using Com.Danliris.Service.Sales.Lib.ViewModels.IntegrationViewModel;
using Com.Danliris.Service.Sales.Lib.ViewModels.SalesInvoice;
using Com.Danliris.Service.Sales.WebApi.Controllers;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Sales.Test.WebApi.Controllers
{
    public class SalesInvoiceControllerTest : BaseControllerTest<SalesInvoiceController, SalesInvoiceModel, SalesInvoiceViewModel, ISalesInvoiceContract>
    {
        [Fact]
        public void Get_Delivery_Order_PDF_Success()
        {
            var vm = new SalesInvoiceViewModel()
            {
                DeliveryOrderNo = "DeliveryOrderNo",
                SalesInvoiceNo = "SalesInvoiceNo",
                SalesInvoiceDate = DateTimeOffset.Now,
                Buyer = new BuyerViewModel()
                {
                    Name = "BuyerName",
                    Code = "BuyerCode",
                    Address = "BuyerAddress",
                },
                Remark = "Remark",

                SalesInvoiceDetails = new List<SalesInvoiceDetailViewModel>()
                {
                    new SalesInvoiceDetailViewModel()
                    {
                        SalesInvoiceItems = new List<SalesInvoiceItemViewModel>()
                        {
                            new SalesInvoiceItemViewModel()
                            {
                                ProductName = "ProductName",
                                ItemUom = "MTR",
                                QuantityPacking = 1,
                                PackingUom = "PackingUom",
                                QuantityItem = 1,
                            }
                        }
                    }
                }

            };
            var mocks = GetMocks();
            mocks.Facade.Setup(x => x.ReadByIdAsync(It.IsAny<int>())).ReturnsAsync(Model);
            mocks.Mapper.Setup(s => s.Map<SalesInvoiceViewModel>(It.IsAny<SalesInvoiceModel>()))
                .Returns(vm);
            var controller = GetController(mocks);
            var response = controller.GetDeliveryOrderPDF(1).Result;

            Assert.NotNull(response);

        }

        [Fact]
        public void Get_Delivery_Order_PDF_NotFound()
        {
            var mocks = GetMocks();
            mocks.Facade.Setup(x => x.ReadByIdAsync(It.IsAny<int>())).ReturnsAsync(default(SalesInvoiceModel));
            var controller = GetController(mocks);
            var response = controller.GetDeliveryOrderPDF(1).Result;

            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);

        }

        [Fact]
        public void Get_Delivery_Order_PDF_Exception()
        {
            var mocks = GetMocks();
            mocks.Facade.Setup(x => x.ReadByIdAsync(It.IsAny<int>())).ThrowsAsync(new Exception("error"));
            var controller = GetController(mocks);
            var response = controller.GetDeliveryOrderPDF(1).Result;

            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);

        }

        [Fact]
        public void Get_Sales_Invoice_PDF_SalesType_Is_Local_VatType_Is_PPN_Umum_And_CurrencySymbol_Is_IDR()
        {
            var vm = new SalesInvoiceViewModel()
            {
                AutoIncreament = 1,
                Buyer = new BuyerViewModel()
                {
                    Name = "BuyerName",
                    Code = "BuyerCode",
                    Address = "BuyerAddress",
                    NPWP = "BuyerNPWP",
                    NIK = "BuyerNIK",
                },
                SalesType = "Lokal",
                SalesInvoiceNo = "SalesInvoiceNo",
                SalesInvoiceDate = DateTimeOffset.Now,
                DueDate = DateTimeOffset.Now,
                Currency = new CurrencyViewModel()
                {
                    Symbol = "Rp",
                },
                PaymentType = "Meter",
                Remark = "Remark",
                VatType = "PPN Umum",
                SalesInvoiceDetails = new List<SalesInvoiceDetailViewModel>()
                {
                    new SalesInvoiceDetailViewModel()
                    {
                        SalesInvoiceItems = new List<SalesInvoiceItemViewModel>()
                        {
                            new SalesInvoiceItemViewModel()
                            {
                                ProductCode = "ProductCode",
                                QuantityPacking = 1,
                                PackingUom = "PackingUom",
                                ItemUom = "ItemUom",
                                ProductName = "ProductName",
                                QuantityItem = 1,
                                Price = 1,
                                Amount = 1,
                            }
                        }
                    }
                }

            };
            var mocks = GetMocks();
            mocks.Facade.Setup(x => x.ReadByIdAsync(It.IsAny<int>())).ReturnsAsync(Model);
            mocks.Mapper.Setup(s => s.Map<SalesInvoiceViewModel>(It.IsAny<SalesInvoiceModel>()))
                .Returns(vm);
            var controller = GetController(mocks);
            var response = controller.GetSalesInvoicePDF(1).Result;

            Assert.NotNull(response);
        }

        [Fact]
        public void Get_Sales_Invoice_PDF_SalesType_Is_Local_VatType_Is_PPN_BUMN_And_CurrencySymbol_Is_USD()
        {
            var vm = new SalesInvoiceViewModel()
            {
                Buyer = new BuyerViewModel()
                {
                    Name = "BuyerName",
                    Code = "BuyerCode",
                    Address = "BuyerAddress",
                    NPWP = "BuyerNPWP",
                    NIK = "BuyerNIK",
                },
                SalesType = "Lokal",
                SalesInvoiceNo = "SalesInvoiceNo",
                SalesInvoiceDate = DateTimeOffset.Now,
                DueDate = DateTimeOffset.Now,
                Currency = new CurrencyViewModel()
                {
                    Symbol = "$",
                },
                Remark = "Remark",
                PaymentType = "Yard",
                VatType = "PPN BUMN",
                SalesInvoiceDetails = new List<SalesInvoiceDetailViewModel>()
                {
                    new SalesInvoiceDetailViewModel()
                    {
                        SalesInvoiceItems = new List<SalesInvoiceItemViewModel>()
                        {
                            new SalesInvoiceItemViewModel()
                            {
                                ProductCode = "ProductCode",
                                QuantityPacking = 1,
                                PackingUom = "PackingUom",
                                ItemUom = "ItemUom",
                                ProductName = "ProductName",
                                QuantityItem = 1,
                                Price = 1,
                                Amount = 1,
                            }
                        }
                    }
                }

            };
            var mocks = GetMocks();
            mocks.Facade.Setup(x => x.ReadByIdAsync(It.IsAny<int>())).ReturnsAsync(Model);
            mocks.Mapper.Setup(s => s.Map<SalesInvoiceViewModel>(It.IsAny<SalesInvoiceModel>()))
                .Returns(vm);
            var controller = GetController(mocks);
            var response = controller.GetSalesInvoicePDF(1).Result;

            Assert.NotNull(response);
        }

        [Fact]
        public void Get_Sales_Invoice_PDF_SalesType_Is_Local_VatType_Is_PPN_Retail_And_CurrencySymbol_Is_EUR()
        {
            var vm = new SalesInvoiceViewModel()
            {
                Buyer = new BuyerViewModel()
                {
                    Name = "BuyerName",
                    Code = "BuyerCode",
                    Address = "BuyerAddress",
                    NPWP = "BuyerNPWP",
                    NIK = "BuyerNIK",
                },
                SalesType = "Lokal",
                SalesInvoiceNo = "SalesInvoiceNo",
                SalesInvoiceDate = DateTimeOffset.Now,
                DueDate = DateTimeOffset.Now,
                Currency = new CurrencyViewModel()
                {
                    Symbol = "€",
                },
                Remark = "Remark",
                VatType = "PPN Retail",
                SalesInvoiceDetails = new List<SalesInvoiceDetailViewModel>()
                {
                    new SalesInvoiceDetailViewModel()
                    {
                        SalesInvoiceItems = new List<SalesInvoiceItemViewModel>()
                        {
                            new SalesInvoiceItemViewModel()
                            {
                                ProductCode = "ProductCode",
                                QuantityPacking = 1,
                                PackingUom = "PackingUom",
                                ItemUom = "ItemUom",
                                ProductName = "ProductName",
                                QuantityItem = 1,
                                Price = 1,
                                Amount = 1,
                            }
                        }
                    }
                }

            };
            var mocks = GetMocks();
            mocks.Facade.Setup(x => x.ReadByIdAsync(It.IsAny<int>())).ReturnsAsync(Model);
            mocks.Mapper.Setup(s => s.Map<SalesInvoiceViewModel>(It.IsAny<SalesInvoiceModel>()))
                .Returns(vm);
            var controller = GetController(mocks);
            var response = controller.GetSalesInvoicePDF(1).Result;

            Assert.NotNull(response);
        }

        [Fact]
        public void Get_Sales_Invoice_PDF_SalesType_Is_Export()
        {
            var vm = new SalesInvoiceViewModel()
            {
                SalesType = "Ekspor",
                SalesInvoiceNo = "SalesInvoiceNo",
                SalesInvoiceDate = DateTimeOffset.Now,
                ShippedPer = "ShippedPer",
                SailingDate = DateTimeOffset.Now,
                Color = "Color",
                OrderNo = "OrderNo",
                Indent = "Indent",
                QuantityLength = 100,
                PaymentType = "USD",
                CartonNo = "CartonNo",
                GrossWeight = 100,
                NetWeight = 100,
                WeightUom = "KG",
                TotalMeas = 100,
                TotalUom = "CBM",
                Sales = "Sales",
                Buyer = new BuyerViewModel()
                {
                    Name = "BuyerName",
                    Address = "BuyerAddress",
                },
                Currency = new CurrencyViewModel()
                {
                    Symbol = "$",
                },
                SalesInvoiceDetails = new List<SalesInvoiceDetailViewModel>()
                {
                    new SalesInvoiceDetailViewModel()
                    {
                        SalesInvoiceItems = new List<SalesInvoiceItemViewModel>()
                        {
                            new SalesInvoiceItemViewModel()
                            {
                                QuantityItem = 10,
                                Price = 10,
                                ProductName = "ProductName",
                            }
                        }
                    }
                }

            };
            var mocks = GetMocks();
            mocks.Facade.Setup(x => x.ReadByIdAsync(It.IsAny<int>())).ReturnsAsync(Model);
            mocks.Mapper.Setup(s => s.Map<SalesInvoiceViewModel>(It.IsAny<SalesInvoiceModel>()))
                .Returns(vm);
            var controller = GetController(mocks);
            var response = controller.GetSalesInvoicePDF(1).Result;

            Assert.NotNull(response);
        }

        [Fact]
        public void Get_Sales_Invoice_PDF_NotFound()
        {
            var mocks = GetMocks();
            mocks.Facade.Setup(x => x.ReadByIdAsync(It.IsAny<int>())).ReturnsAsync(default(SalesInvoiceModel));
            var controller = GetController(mocks);
            var response = controller.GetSalesInvoicePDF(1).Result;

            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);

        }

        [Fact]
        public void Get_Sales_Invoice_PDF_Exception()
        {
            var mocks = GetMocks();
            mocks.Facade.Setup(x => x.ReadByIdAsync(It.IsAny<int>())).ThrowsAsync(new Exception("error"));
            var controller = GetController(mocks);
            var response = controller.GetSalesInvoicePDF(1).Result;

            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);

        }

        [Fact]
        public void Mapping_With_AutoMapper_Profiles()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<SalesInvoiceMapper>();
                cfg.AddProfile<SalesInvoiceDetailMapper>();
                cfg.AddProfile<SalesInvoiceItemMapper>();
            });
            var mapper = configuration.CreateMapper();

            SalesInvoiceViewModel salesInvoiceViewModel = new SalesInvoiceViewModel { Id = 1 };
            SalesInvoiceModel salesInvoiceModel = mapper.Map<SalesInvoiceModel>(salesInvoiceViewModel);

            Assert.Equal(salesInvoiceViewModel.Id, salesInvoiceModel.Id);

            SalesInvoiceDetailViewModel salesInvoiceDetailViewModel = new SalesInvoiceDetailViewModel { Id = 1 };
            SalesInvoiceDetailModel salesInvoiceDetailModel = mapper.Map<SalesInvoiceDetailModel>(salesInvoiceDetailViewModel);

            Assert.Equal(salesInvoiceDetailViewModel.Id, salesInvoiceDetailModel.Id);

            SalesInvoiceItemViewModel salesInvoiceItemViewModel = new SalesInvoiceItemViewModel { Id = 1 };
            SalesInvoiceItemModel salesInvoiceItemModel = mapper.Map<SalesInvoiceItemModel>(salesInvoiceItemViewModel);

            Assert.Equal(salesInvoiceItemViewModel.Id, salesInvoiceItemModel.Id);
        }

        [Fact]
        public void Validate_Validation_ViewModel()
        {
            List<SalesInvoiceViewModel> viewModels = new List<SalesInvoiceViewModel>
            {
                new SalesInvoiceViewModel(){},
                new SalesInvoiceViewModel()
                {
                    SalesInvoiceDetails = new List<SalesInvoiceDetailViewModel>()
                    {
                        new SalesInvoiceDetailViewModel() {},
                        new SalesInvoiceDetailViewModel() {
                            SalesInvoiceItems = new List<SalesInvoiceItemViewModel>()
                            {
                                new SalesInvoiceItemViewModel() {},
                            }
                        },
                    }
                },
                new SalesInvoiceViewModel
                {
                    SalesInvoiceType = null,
                    PaymentType = null,
                    VatType = null,
                    DueDate = null,
                },
                new SalesInvoiceViewModel
                {
                    SalesInvoiceType = "",
                    SalesType = "",
                    SalesInvoiceCategory = "DYEINGPRINTING",
                    SalesInvoiceDetails = new List<SalesInvoiceDetailViewModel>()
                    {
                        new SalesInvoiceDetailViewModel()
                        {
                        ShippingOutId = null,
                        BonNo = null,
                        }
                    }
                },
                new SalesInvoiceViewModel
                {
                    SalesInvoiceType = "",
                    PaymentType = "",
                    VatType = "",
                    DueDate = DateTimeOffset.Now.AddDays(-1),
                    SalesInvoiceDate = DateTimeOffset.UtcNow.AddDays(1),
                    Currency = new CurrencyViewModel()
                    {
                        Id = 0,
                        Code = "",
                        Rate = 0,
                    },
                    Buyer = new BuyerViewModel()
                    {
                        Id = 0,
                        Name = "",
                        Code = "",
                    },
                    Unit = new UnitViewModel()
                    {
                        Id = 0,
                        Code = "",
                        Name = "",
                    },
                    TotalPayment = 0,
                    TotalPaid = -1,
                    SalesType = "Ekspor",
                    SailingDate = null,
                    ShippedPer = "",
                    Color = "",
                    OrderNo = "",
                    Indent = "",
                    CartonNo = "",
                    WeightUom = "",
                    TotalUom = "",
                    QuantityLength = 0,
                    GrossWeight = 0,
                    NetWeight = 0,
                    TotalMeas = 0,
                    SalesInvoiceDetails = new List<SalesInvoiceDetailViewModel>()
                    {
                        new SalesInvoiceDetailViewModel()
                        {
                        ShippingOutId = 0,
                        BonNo = "NewBonNo",
                            SalesInvoiceItems = new List<SalesInvoiceItemViewModel>()
                            {
                                new SalesInvoiceItemViewModel()
                                {
                                    ProductId = null,
                                    ProductCode = "",
                                    ProductName = "",
                                    Amount = 0,
                                    PackingUom = "",
                                    ItemUom = "",
                                    QuantityPacking = -1,
                                    QuantityItem = -1,
                                    ConvertUnit = "",
                                    ConvertValue = -1,
                                },
                                new SalesInvoiceItemViewModel()
                                {
                                }
                            }
                        },
                        new SalesInvoiceDetailViewModel()
                        {
                        ShippingOutId = 10,
                        BonNo = "NewBonNo",
                            SalesInvoiceItems = new List<SalesInvoiceItemViewModel>()
                            {
                                new SalesInvoiceItemViewModel()
                                {
                                    ProductCode = null,
                                    Price = -1,
                                },
                                new SalesInvoiceItemViewModel()
                                {
                                    Price = null,
                                }
                            }
                        }
                    }
                },
            };
            foreach (var viewModel in viewModels)
            {
                var defaultValidationResult = viewModel.Validate(null);
                Assert.True(defaultValidationResult.Count() > 0);
            }
        }

        [Fact]
        public void Validate_Validation_For_Export()
        {
            List<SalesInvoiceViewModel> viewModels = new List<SalesInvoiceViewModel>
            {
                new SalesInvoiceViewModel
                {
                    SalesType = "Ekspor",
                    SailingDate = null,
                    ShippedPer = null,
                    Color = null,
                    OrderNo = null,
                    Indent = null,
                    CartonNo = null,
                    //WeightUom = null,
                    //TotalUom = null,
                    QuantityLength = null,
                    GrossWeight = null,
                    NetWeight = null,
                    TotalMeas = null,
                    SalesInvoiceDetails = new List<SalesInvoiceDetailViewModel>()
                    {
                        new SalesInvoiceDetailViewModel()
                        {
                            SalesInvoiceItems = new List<SalesInvoiceItemViewModel>()
                            {
                                new SalesInvoiceItemViewModel()
                                {
                                },
                                new SalesInvoiceItemViewModel()
                                {
                                }
                            },
                        },
                    },
                },
            };
            foreach (var viewModel in viewModels)
            {
                var defaultValidationResult = viewModel.Validate(null);
                Assert.True(defaultValidationResult.Count() > 0);
            }
        }

        [Fact]
        public void Validate_Duplicate_DetailViewModel()
        {
            List<SalesInvoiceViewModel> viewModels = new List<SalesInvoiceViewModel>
            {
                new SalesInvoiceViewModel {
                    DueDate = DateTimeOffset.Now,
                    SalesInvoiceCategory = "DYEINGPRINTING",
                    Currency = new CurrencyViewModel() {},
                    SalesInvoiceDetails = new List<SalesInvoiceDetailViewModel>()
                    {
                        new SalesInvoiceDetailViewModel()
                        {
                            ShippingOutId = 1,
                            BonNo ="BonNo",
                            SalesInvoiceItems = new List<SalesInvoiceItemViewModel>()
                            {
                                new SalesInvoiceItemViewModel()
                                {
                                    Id = 2,
                                    ProductCode = "ProductCode",
                                    QuantityPacking = 100,
                                    PackingUom = "PackingUom",
                                    QuantityItem = 10,
                                    ItemUom = "MTR",
                                    ProductName = "ProductName",
                                    Price = 100,
                                    Amount = 100,
                                },
                                new SalesInvoiceItemViewModel()
                                {
                                    Id = 2,
                                    ProductCode = "ProductCode",
                                    QuantityPacking = 100,
                                    PackingUom = "PackingUom",
                                    QuantityItem = 10,
                                    ItemUom = "MTR",
                                    ProductName = "ProductName",
                                    Price = 100,
                                    Amount = 100,
                                },
                            }
                        },
                        new SalesInvoiceDetailViewModel()
                        {
                            ShippingOutId = 1,
                            BonNo ="BonNo",
                        }
                    }
                }
            };
            foreach (var viewModel in viewModels)
            {
                var defaultValidationResult = viewModel.Validate(null);
                Assert.True(defaultValidationResult.Count() > 0);
            }
        }


        [Fact]
        public void Should_Success_Read_By_Buyer()
        {
            var mocks = GetMocks();
            mocks.Facade.Setup(f => f.ReadByBuyerId(It.IsAny<int>())).Returns(new List<SalesInvoiceModel>() { new SalesInvoiceModel() });
            mocks.Mapper.Setup(m => m.Map<List<SalesInvoiceViewModel>>(It.IsAny<List<SalesInvoiceModel>>())).Returns(new List<SalesInvoiceViewModel>());
            var controller = GetController(mocks);
            var response = controller.ReadByBuyerId(It.IsAny<int>());
            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void Should_ReturnFailed_Read_By_Buyer_ThrowException()
        {
            var mocks = GetMocks();
            mocks.Facade.Setup(f => f.ReadByBuyerId(It.IsAny<int>())).Returns(new List<SalesInvoiceModel>() { new SalesInvoiceModel() });
            mocks.Mapper.Setup(m => m.Map<List<SalesInvoiceViewModel>>(It.IsAny<List<SalesInvoiceModel>>())).Throws(new Exception());
            var controller = GetController(mocks);
            var response = controller.ReadByBuyerId(It.IsAny<int>());
            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task Should_Success_Update_From_Sales_Receipt()
        {
            var mocks = this.GetMocks();
            mocks.ValidateService.Setup(vs => vs.Validate(It.IsAny<SalesInvoiceViewModel>())).Verifiable();
            mocks.Facade.Setup(f => f.UpdateFromSalesReceiptAsync(It.IsAny<int>(), It.IsAny<SalesInvoiceUpdateModel>())).ReturnsAsync(1);
            var controller = GetController(mocks);
            var response = await controller.UpdateFromSalesReceiptAsync(It.IsAny<int>(), It.IsAny<SalesInvoiceUpdateModel>());
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task Should_Fail_Update_From_Sales_Receipt()
        {
            var mocks = this.GetMocks();
            mocks.ValidateService.Setup(vs => vs.Validate(It.IsAny<SalesInvoiceViewModel>())).Verifiable();
            mocks.Facade.Setup(f => f.UpdateFromSalesReceiptAsync(It.IsAny<int>(), It.IsAny<SalesInvoiceUpdateModel>()))
                .ThrowsAsync(new Exception());
            var controller = GetController(mocks);
            var response = await controller.UpdateFromSalesReceiptAsync(It.IsAny<int>(), It.IsAny<SalesInvoiceUpdateModel>());
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async void Should_Success_GetReportAll()
        {
            var mocks = GetMocks();
            mocks.Facade.Setup(f => f.GetReport(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<bool?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<int>()))
                .ReturnsAsync(new List<SalesInvoiceReportViewModel>());

            var controller = GetController(mocks);
            var response = await controller.GetReportAll(0, 0, null, null, null, "7");
            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async void Should_Fail_GetReportAll()
        {
            var mocks = GetMocks();
            mocks.Facade.Setup(f => f.GetReport(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<bool?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<int>()))
                .Throws(new Exception());

            var controller = GetController(mocks);
            var response = await controller.GetReportAll(0, 0, null, null, null, "7");
            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async void Should_Success_GetXlsAll()
        {
            var mocks = GetMocks();
            mocks.Facade.Setup(f => f.GenerateExcel(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<bool?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<int>()))
                .ReturnsAsync(new System.IO.MemoryStream());

            var controller = GetController(mocks);
            var response = await controller.GetXlsAll(0, 0, null, null, null, "7");
            Assert.NotNull(response);
        }

        [Fact]
        public async void Should_Fail_GetXlsAll()
        {
            var mocks = GetMocks();
            mocks.Facade.Setup(f => f.GenerateExcel(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<bool?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<int>()))
               .Throws(new Exception());

            var controller = GetController(mocks);
            var response = await controller.GetXlsAll(0, 0, null, null, null, "7");
            int statusCode = GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }
    }
}
