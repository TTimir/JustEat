<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Invoice1.aspx.cs" Inherits="JustEat.Users.Invoice1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>JustEat - Order Invoice</title>
    <link rel="shortcut icon" href="../TemplateFiles/images/favicon.png" type="" />
    <link href="assets/images/favicon/icon.png" rel="icon" />
    <%--<link href="../css2?family=Inter:wght@100;200;300;400;500;600;700;800;900&family=Lato:ital,wght@0,100;0,300;0,400;0,700;0,900;1,100;1,300;1,400;1,700;1,900&display=swap" rel="stylesheet" />--%>
    <link href="../assets/Invoice/css/custom.css" rel="stylesheet" />
    <link href="../assets/Invoice/css/media-query.css" rel="stylesheet" />
    <style>
        .invoice-logo-content {
            font-family: 'Dancing Script', cursive;
        }

        .restaurant-logo span {
            font-weight: bold;
            font-size: 32px;
            color: #ffffff;
        }
    </style>
</head>
<body>

    <!--Invoice wrap start here -->
    <div class="invoice_wrap restaurant">
        <div class="invoice-container">
            <div class="invoice-content-wrap" id="download_section">
                <!--Header start here -->
                <header class="bg-yellow restaurant-header" id="invo_header">
                    <div class="invoice-logo-content">
                        <div class="invoice-logo-details wid-50">
                            <a href="Default.aspx" class="restaurant-logo">
                                <span>JustEat
                                </span>
                            </a>
                            <div class="res-contact">
                                <div class="invo-cont-wrap invo-contact-wrap">
                                    <div class="invo-social-icon">
                                        <svg width="24" height="24" viewbox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                            <g clip-path="url(#clip0_6_94)">
                                                <path d="M5 4H9L11 9L8.5 10.5C9.57096 12.6715 11.3285 14.429 13.5 15.5L15 13L20 15V19C20 19.5304 19.7893 20.0391 19.4142 20.4142C19.0391 20.7893 18.5304 21 18 21C14.0993 20.763 10.4202 19.1065 7.65683 16.3432C4.8935 13.5798 3.23705 9.90074 3 6C3 5.46957 3.21071 4.96086 3.58579 4.58579C3.96086 4.21071 4.46957 4 5 4" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path>
                                                <path d="M15 7C15.5304 7 16.0391 7.21071 16.4142 7.58579C16.7893 7.96086 17 8.46957 17 9" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path>
                                                <path d="M15 3C16.5913 3 18.1174 3.63214 19.2426 4.75736C20.3679 5.88258 21 7.4087 21 9" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path>
                                            </g><defs><clippath id="clip0_6_94"><rect width="24" height="24" fill="white"></rect>
                                            </clippath>
                                            </defs></svg>
                                    </div>
                                    <div class="invo-social-name">
                                        <a href="tel:+12345678899" class="font-18">+91 234 567 8899</a>
                                    </div>
                                </div>
                                <div class="invo-cont-wrap pt-10">
                                    <div class="invo-social-icon">
                                        <svg width="24" height="24" viewbox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                            <g clip-path="url(#clip0_6_108)">
                                                <path d="M19 5H5C3.89543 5 3 5.89543 3 7V17C3 18.1046 3.89543 19 5 19H19C20.1046 19 21 18.1046 21 17V7C21 5.89543 20.1046 5 19 5Z" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path>
                                                <path d="M3 7L12 13L21 7" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path>
                                            </g><defs><clippath id="clip0_6_108"><rect width="24" height="24" fill="white"></rect>
                                            </clippath>
                                            </defs></svg>
                                    </div>
                                    <div class="invo-social-name">
                                        <a href="mailto:contact@invoice.com" class="font-18">contact@invoice.com</a>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="invoice-logo-res wid-50">
                            <a href="restaurant_bill.html" class="logo">
                                <img src="../assets/Invoice/images/hotel/restaurant-header-img.png" alt="this is a invoice logo" /></a>
                        </div>
                    </div>
                </header>
                <!--Header end here -->
                <!--Invoice content start here -->
                <section class="agency-service-content restaurant-invoice-content book_section layout_padding" id="restaurant_bill">
                    <div class="container">
                        <div class="heading_container">
                            <div class="align-self-end">
                                <asp:Label ID="lblMsg" runat="server" Visible="false"></asp:Label>
                            </div>
                        </div>
                    </div>
                    <div class="container">
                        <div class="bus-invo-no-date-wrap">
                            <div class="bus-invo-num">
                                <span class="font-md color-light-black">Invoice No: <span class="font-md-grey color-light-black">#</span></span>
                                <asp:Repeater ID="rOrderItem5" runat="server">
                                    <ItemTemplate>
                                        <span class="font-md-grey color-light-black"><%# Regex.Replace(Eval("OrderDetailsId").ToString().ToLower(), @"\s+", "") %></span>
                                    </ItemTemplate>
                                </asp:Repeater>
                            </div>
                            <div class="bus-invo-date">
                                <span class="font-md color-light-black">Invoice Date:</span>
                                <span class="font-md-grey color-light-black"><%= DateTime.Now.ToString("dd/MM/yyyy") %></span>
                            </div>
                        </div>
                        <!--Invoice owner name start here -->
                        <div class="invoice-owner-conte-wrap pt-20">
                            <div class="invo-to-wrap">
                                <div class="invoice-to-content">
                                    <p class="font-md color-light-black">Invoice To:</p>
                                    <h1 class="font-lg color-yellow pt-10" id="lblUserName" runat="server"></h1>
                                    <p class="font-md-grey color-grey pt-10">
                                        Phone: <span id="lblUserPhone" runat="server"></span>
                                        <br />
                                        Email: <span id="lblUserEmail" runat="server"></span>
                                    </p>
                                </div>
                            </div>
                            <div class="invo-pay-to-wrap">
                                <div class="invoice-pay-content">
                                    <p class="font-md color-light-black">Pay To:</p>
                                    <h6 class="font-lg color-yellow pt-10">JustEat</h6>
                                    <p class="font-md-grey color-grey pt-10">
                                        4510 E Dolphine St, IN 3526<br>
                                        Hills Road, Bangaluru, India
                                    </p>
                                </div>
                            </div>
                        </div>
                        <!--Invoice owner name end here -->
                        <!--Invoice table data start here -->
                        <div class="table-wrapper res-contact">
                            <asp:Repeater ID="rOrderItem1" runat="server">
                                <HeaderTemplate>
                                    <table class="invoice-table restaurant-table">
                                        <thead>
                                            <tr class="invo-tb-header bg-yellow">
                                                <th class="font-md color-light-black res-no  pl-10">S. No.</th>
                                                <th class="font-md color-light-black w-40">Order Number</th>
                                                <th class="font-md color-light-black res-pric text-center">Item Name</th>
                                                <th class="font-md color-light-black res-qty text-center">Unit Price</th>
                                                <th class="font-md color-light-black res-total text-center ">Qty</th>
                                                <th class="font-md color-light-black res-total text-center ">Total Price</th>
                                            </tr>
                                        </thead>
                                        <tbody class="invo-tb-body">
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <tr class="invo-tb-row">
                                        <td runat="server" id="tdSrno" cssclass="font-sm color-grey"><%# Eval("SrNo") %></td>
                                        <td runat="server" id="tdOrderNo" cssclass="font-sm color-grey" style="padding: 15px;"><%# Eval("OrderNo") %></td>
                                        <td runat="server" id="tdName" cssclass="font-sm color-grey text-center" style="padding: 15px;"><%# Eval("Name") %></td>
                                        <td runat="server" id="tdPrice" cssclass="font-sm color-grey text-center" style="padding: 15px;"><%# string.IsNullOrEmpty( Eval("Price").ToString() ) ? "" : "₹"+ Eval("Price") %></td>
                                        <td runat="server" id="tdQuantity" cssclass="font-sm color-grey text-center" style="padding: 15px;"><%# Eval("Quantity") %></td>
                                        <td runat="server" id="tdTotalPrice" cssclass="font-sm color-grey text-center" style="padding: 15px;">₹<%# Eval("TotalPrice") %></td>
                                    </tr>
                                </ItemTemplate>
                                <FooterTemplate>
                                    <tr class="invo-tb-footer">
                                        <td colspan="5" class="text-right color-yellow font-18-700 pt-20">Grand Total:</td>
                                        <td class="font-18-500 color-light-black pt-20 text-center" style="padding: 15px;">₹<% Response.Write(Session["grandTotalPrice"]); %></td>
                                    </tr>
                                    </tbody>
                                </table>
                                </FooterTemplate>
                            </asp:Repeater>

                        </div>
                        <!--Invoice table data end here -->
                        <!--Invoice additional info start here -->
                        <div class="invo-addition-wrap pt-20">
                            <div>
                                <h3 class="font-md color-light-black">Additional Information:</h3>


                                <p class="font-sm pt-10">
                                    Thank you for choosing JustEat! We hope you enjoy your meal. Our
                                <asp:Repeater ID="rOrderItem2" runat="server">
                                    <ItemTemplate>
                                        <%# Eval("Name") %>
                                        <asp:Literal runat="server" Text='<%# Container.ItemIndex < (Container.Parent as Repeater).Items.Count - 1 ? "- " : ", " %>'></asp:Literal>
                                    </ItemTemplate>
                                </asp:Repeater>
                                    are made with the freshest ingredients and cooked to perfection. If you have any feedback or need assistance with your order, please don't hesitate to contact us. Your satisfaction is our top priority.
                                </p>


                            </div>
                        </div>
                        <!--Invoice additional info end here -->
                        <!--Payment detail table start here -->
                        <div class="rest-payment-bill pt-20">
                            <div class="payment-wrap payment-wrap-res p-0">
                                <table class="res-pay-table">
                                    <tbody>
                                        <tr class="table-bg">
                                            <td class="font-md color-light-black pay-type" style="padding: 15px;">Payment Details:</td>
                                            <td style="padding: 15px;">
                                                <div>
                                                    <asp:Label ID="lblPaymentMode" runat="server" CssClass="font-md-grey color-grey"></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="font-md color-light-black pay-type" style="padding: 15px;">Order Number (Last 4 Digits):</td>
                                            <td class="font-md-grey color-grey" style="padding: 15px;">
                                                <asp:Repeater ID="rOrderItem4" runat="server">
                                                    <ItemTemplate>
                                                        <%# Eval("OrderNo").ToString().Length > 4 ? Eval("OrderNo").ToString().Substring(Eval("OrderNo").ToString().Length - 4) : Eval("OrderNo").ToString() %>
                                                        <asp:Literal runat="server" Text='<%# Container.ItemIndex < (Container.Parent as Repeater).Items.Count - 1 ? "- " : ", " %>'></asp:Literal>
                                                    </ItemTemplate>
                                                </asp:Repeater>
                                            </td>
                                        </tr>
                                        <tr class="table-bg">
                                            <td class="font-md color-light-black pay-type" style="padding: 15px;">Date:</td>
                                            <td style="padding: 15px;">
                                                <div>
                                                    <asp:Label ID="lblOrderDate" runat="server" CssClass="font-md-grey color-grey"></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="font-md color-light-black pay-type" style="padding: 15px;">Amount(₹):</td>
                                            <td style="padding: 15px;">
                                                <div>
                                                    <asp:Label ID="lblGrandTotal" runat="server" CssClass="font-md-grey color-grey"></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </div>
                            <div class="signature-wrap">
                                <div class="sign-img">
                                    <img src="../assets/Invoice/images/signature/signature.png" alt="this is signature image" />
                                </div>
                                <p class="font-sm-500">Just Eat</p>
                                <h3 class="font-md-grey color-light-black">Food Court Manager</h3>
                            </div>
                        </div>

                        <!--Payment detail table end here -->
                        <div class="res-contact res-bottom">
                            <p class="font-sm color-light-black text-center">Thank you for choosing to dine with us. See you soon 🙂</p>
                        </div>

                        <!--Note content start here -->
                        <div class="invo-note-wrap">
                            <div class="note-title">
                                <svg width="24" height="24" viewbox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                    <g clip-path="url(#clip0_8_240)">
                                        <path d="M14 3V7C14 7.26522 14.1054 7.51957 14.2929 7.70711C14.4804 7.89464 14.7348 8 15 8H19" stroke="#12151C" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path>
                                        <path d="M17 21H7C6.46957 21 5.96086 20.7893 5.58579 20.4142C5.21071 20.0391 5 19.5304 5 19V5C5 4.46957 5.21071 3.96086 5.58579 3.58579C5.96086 3.21071 6.46957 3 7 3H14L19 8V19C19 19.5304 18.7893 20.0391 18.4142 20.4142C18.0391 20.7893 17.5304 21 17 21Z" stroke="#12151C" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path>
                                        <path d="M9 7H10" stroke="#12151C" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path>
                                        <path d="M9 13H15" stroke="#12151C" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path>
                                        <path d="M13 17H15" stroke="#12151C" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path>
                                    </g><defs><clippath id="clip0_8_240"><rect width="24" height="24" fill="white"></rect>
                                    </clippath>
                                    </defs></svg>
                                <span class="font-md color-light-black">Note:</span>
                            </div>
                            <h2 class="font-md-grey color-grey note-desc">This is computer generated receipt and does not require physical signature.</h2>
                        </div>
                        <!--Note content end here -->
                    </div>
                </section>
                <!--Invoice content end here -->
            </div>
            <!--Bottom content start here -->
            <section class="agency-bottom-content d-print-none" id="agency_bottom">
                <div class="container">
                    <!--Print-download content start here -->
                    <div class="invo-buttons-wrap">
                        <div class="invo-print-btn invo-btns">
                            <a href="javascript:window.print()" class="print-btn">
                                <svg width="24" height="24" viewbox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                    <g clip-path="url(#clip0_5_313)">
                                        <path d="M14 3V7C14 7.26522 14.1054 7.51957 14.2929 7.70711C14.4804 7.89464 14.7348 8 15 8H19" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path>
                                        <path d="M17 21H7C6.46957 21 5.96086 20.7893 5.58579 20.4142C5.21071 20.0391 5 19.5304 5 19V5C5 4.46957 5.21071 3.96086 5.58579 3.58579C5.96086 3.21071 6.46957 3 7 3H14L19 8V19C19 19.5304 18.7893 20.0391 18.4142 20.4142C18.0391 20.7893 17.5304 21 17 21Z" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path>
                                        <path d="M9 7H10" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path>
                                        <path d="M9 13H15" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path>
                                        <path d="M13 17H15" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path>
                                    </g><defs><clippath id="clip0_5_313"><rect width="24" height="24" fill="white"></rect>
                                    </clippath>
                                    </defs>
                                </svg>
                                <span class="inter-700 medium-font">Print</span>
                            </a>
                        </div>
                        <div class="invo-down-btn invo-btns">
                            <a class="download-btn" id="generatePDF">
                                <svg width="24" height="24" viewbox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                    <g clip-path="url(#clip0_5_246)">
                                        <path d="M4 17V19C4 19.5304 4.21071 20.0391 4.58579 20.4142C4.96086 20.7893 5.46957 21 6 21H18C18.5304 21 19.0391 20.7893 19.4142 20.4142C19.7893 20.0391 20 19.5304 20 19V17" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path>
                                        <path d="M7 11L12 16L17 11" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path>
                                        <path d="M12 4V16" stroke="white" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path>
                                    </g><defs><clippath id="clip0_5_246"><rect width="24" height="24" fill="white"></rect>
                                    </clippath>
                                    </defs>
                                </svg>
                                <span class="inter-700 medium-font color-white">Download</span>
                            </a>
                        </div>
                    </div>
                    <!--Print-download content end here -->
                    <!--Note content start here -->
                    <div class="invo-note-wrap">
                        <div class="note-title">
                            <svg width="24" height="24" viewbox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg">
                                <g clip-path="url(#clip0_8_240)">
                                    <path d="M14 3V7C14 7.26522 14.1054 7.51957 14.2929 7.70711C14.4804 7.89464 14.7348 8 15 8H19" stroke="#12151C" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path>
                                    <path d="M17 21H7C6.46957 21 5.96086 20.7893 5.58579 20.4142C5.21071 20.0391 5 19.5304 5 19V5C5 4.46957 5.21071 3.96086 5.58579 3.58579C5.96086 3.21071 6.46957 3 7 3H14L19 8V19C19 19.5304 18.7893 20.0391 18.4142 20.4142C18.0391 20.7893 17.5304 21 17 21Z" stroke="#12151C" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path>
                                    <path d="M9 7H10" stroke="#12151C" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path>
                                    <path d="M9 13H15" stroke="#12151C" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path>
                                    <path d="M13 17H15" stroke="#12151C" stroke-width="2" stroke-linecap="round" stroke-linejoin="round"></path>
                                </g><defs><clippath id="clip0_8_240"><rect width="24" height="24" fill="white"></rect>
                                </clippath>
                                </defs></svg>
                            <span class="font-md color-light-black">Note:</span>
                        </div>
                        <h3 class="font-md-grey color-grey note-desc">This is computer generated receipt and does not require physical signature.</h3>
                    </div>
                    <!--Note content end here -->
                </div>
            </section>
            <!--bottom content end here -->
        </div>
    </div>
    <!--Invoice wrap End here -->

    <script src="../assets/Invoice/js/jquery.min.js"></script>
    <script src="../assets/Invoice/js/jspdf.min.js"></script>
    <script src="../assets/Invoice/js/html2canvas.min.js"></script>
    <script src="../assets/Invoice/js/custom.js"></script>
    <script>
        //For disappearing alert message
        window.onload = function () {
            var seconds = 5;
            setTimeout(function () {
                document.getElementById("<%=lblMsg.ClientID %>").style.display = "none";
            }, seconds * 1000);
        }
    </script>
</body>
</html>
