<%@ Page Title="" Language="C#" MasterPageFile="~/Users/UserMaster.Master" AutoEventWireup="true" CodeBehind="Profile.aspx.cs" Inherits="JustEat.Users.Profile" %>

<%@ Import Namespace="JustEat" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <%
        string imageUrl = Session["ImageUrl"].ToString();
    %>

    <section class="book_section layout_padding">
        <div class="container">
            <div class="heading_container">
                <h2>User Information</h2>
                <asp:Label ID="lblMessage" runat="server" CssClass="" />
            </div>

            <div class="row">
                <div class="col-12">
                    <div class="card">
                        <div class="card-body">
                            <div class="card-title mb-4">
                                <div class="d-flex justify-content-start">
                                    <div class="image-container">
                                        <img src="<%= Connection.Utils.GetImageUrl(imageUrl) %>" id="imgProfile" style="width: 150px; height: 150px;" class="img-thumbnail" />
                                        <div class="middle pt-2">
                                            <a href="Registration.aspx?id=<% Response.Write(Session["userId"]); %>" class="btn btn-warning">
                                                <i class="fa fa-pencil"></i>Edit Details
                                            </a>
                                        </div>
                                    </div>

                                    <div class="userData ml-3">
                                        <h2 class="d-block" style="font-size: 1.5rem; font-weight: bold">
                                            <a href="javascript:void(0);"><%Response.Write(Session["name"]); %></a>
                                        </h2>
                                        <h6 class="d-block">
                                            <a href="javascript:void(0);">
                                                <asp:Label ID="lblUsername" runat="server" ToolTip="Unique Username">
                                                    @<%Response.Write(Session["username"]); %>
                                                </asp:Label>
                                            </a>
                                        </h6>
                                        <h6 class="d-block">
                                            <a href="javascript:void(0);">
                                                <asp:Label ID="lblEmail" runat="server" ToolTip="User Email">
                                                    <%Response.Write(Session["email"]); %>
                                                </asp:Label>
                                            </a>
                                        </h6>
                                        <h6 class="d-block">
                                            <a href="javascript:void(0);">
                                                <asp:Label ID="lblCreatedDate" runat="server" ToolTip="Account Created On">
                                                    In Kitchan On: <%Response.Write(Session["createdDate"]); %>
                                                </asp:Label>
                                            </a>
                                        </h6>
                                    </div>

                                </div>
                            </div>

                            <div class="row">
                                <div class="col-12">
                                    <ul class="nav nav-tabs mb-4" id="myTab" role="tablist">
                                        <li class="nav-item">
                                            <a class="nav-link active text-info" id="basicInfo-tab" data-toggle="tab" href="#basicInfo" role="tab"
                                                aria-controls="basicInfo" aria-selected="true"><i class="fa fa-id-badge mr-2"></i>Personal Info</a>
                                        </li>
                                        <li class="nav-item">
                                            <a class="nav-link text-info" id="connectedServices-tab" data-toggle="tab" href="#connectedServices" role="tab"
                                                aria-controls="connectedServices" aria-selected="false"><i class="fa fa-clock-o mr-2"></i>Order History</a>
                                        </li>
                                    </ul>

                                    <div class="tab-content ml-1" id="myTabContent">
                                        <!-- Personal Info Starts -->
                                        <div class="tab-pane fade show active" id="basicInfo" role="tabpanel" aria-labelledby="basicInfo-tab">
                                            <asp:Repeater ID="rUserProfile" runat="server">
                                                <ItemTemplate>

                                                    <div class="row">
                                                        <div class="col-sm-3 col-md-2 col-5">
                                                            <label style="font-weight: bold;">Full Name</label>
                                                        </div>
                                                        <div class="col-md-8 col-6">
                                                            <%# Eval("Name") %>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class="row">
                                                        <div class="col-sm-3 col-md-2 col-5">
                                                            <label style="font-weight: bold;">Username</label>
                                                        </div>
                                                        <div class="col-md-8 col-6">
                                                            @<%# Eval("Username") %>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class="row">
                                                        <div class="col-sm-3 col-md-2 col-5">
                                                            <label style="font-weight: bold;">Mobile No.</label>
                                                        </div>
                                                        <div class="col-md-8 col-6">
                                                            <%# Eval("Mobile") %>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class="row">
                                                        <div class="col-sm-3 col-md-2 col-5">
                                                            <label style="font-weight: bold;">Zip/Post Code</label>
                                                        </div>
                                                        <div class="col-md-8 col-6">
                                                            <%# Eval("PostCode") %>
                                                        </div>
                                                    </div>
                                                    <hr />
                                                    <div class="row">
                                                        <div class="col-sm-3 col-md-2 col-5">
                                                            <label style="font-weight: bold;">Address</label>
                                                        </div>
                                                        <div class="col-md-8 col-6">
                                                            <%# Eval("Address") %>
                                                        </div>
                                                    </div>
                                                    <hr />

                                                </ItemTemplate>
                                            </asp:Repeater>
                                        </div>
                                        <!-- Personal Info Ends -->

                                        <!-- Order History Starts -->
                                        <div class="tab-pane fade" id="connectedServices" role="tabpanel" aria-labelledby="ConnectedServices-tab">

                                            <asp:Repeater ID="rPurchaseHistory" runat="server" OnItemDataBound="rPurchaseHistory_ItemDataBound" OnItemCommand="rPurchaseHistory_ItemCommand">
                                                <ItemTemplate>
                                                    <div class="container my-3 p-3 border rounded" style="background-color: #f8f9fa;">
                                                        <div class="row py-2" style="background-color: #e9ecef;">
                                                            <div class="col-4 d-flex align-items-center">
                                                                <span class="badge badge-pill badge-dark text-white mr-2">
                                                                    <%# Eval("SrNo") %> 
                                                                </span>
                                                                <strong>Order Date: </strong> <%# Eval("OrderDate") %>
                                                            </div>
                                                            <div class="col-3 d-flex align-items-center">
                                                                <strong>Pay Mode: </strong> <%# Eval("PaymentMode").ToString() == "cod" ? "Cash On Delivery" : Eval("PaymentMode").ToString().ToUpper() %>
                                                            </div>
                                                            <div class="col-3 d-flex align-items-center">
                                                                <%# string.IsNullOrEmpty(Eval("CardNo").ToString()) ? "" : "<strong>Card No:</strong> " + Eval("CardNo") %>
                                                            </div>
                                                            <div class="col-2 text-right">
                                                                <a href="Invoice1.aspx?id=<%# Eval("PaymentId") %>" class="btn btn-info btn-sm">
                                                                    <i class="fa fa-download mr-2"></i>Invoice</a>
                                                            </div>
                                                        </div>
                                                        <asp:HiddenField ID="hdnPaymentId" Value='<%# Eval("PaymentId") %>' runat="server" />

                                                        <asp:Repeater ID="rOrders" runat="server">
                                                            <HeaderTemplate>
                                                                <table class="table table-striped table-bordered table-hover table-sm mt-3">
                                                                    <thead class="thead-dark">
                                                                        <tr>
                                                                            <th>Product Name</th>
                                                                            <th>Unit Price</th>
                                                                            <th>Qty</th>
                                                                            <th>Total Price</th>
                                                                            <th>Order</th>
                                                                            <th>Status</th>
                                                                        </tr>
                                                                    </thead>
                                                                    <tbody>
                                                            </HeaderTemplate>
                                                            <ItemTemplate>
                                                                <tr>
                                                                    <td>
                                                                        <asp:Label ID="lblName" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblPrice" runat="server" Text='<%# string.IsNullOrEmpty(Eval("Price").ToString()) ? "" : "₹" + Eval("Price") %>'></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblQty" runat="server" Text='<%# Eval("Quantity") %>'></asp:Label>
                                                                    </td>
                                                                    <td>₹<asp:Label ID="lblTotalPrice" runat="server" Text='<%# Eval("TotalPrice") %>'></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblOrderNo" runat="server" Text='<%# Eval("OrderNo") %>'></asp:Label>
                                                                    </td>
                                                                    <td>
                                                                        <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'
                                                                            CssClass='<%# Eval("Status").ToString() == "Prepared" ? "badge badge-success" : "badge badge-warning" %>'></asp:Label>
                                                                    </td>
                                                                </tr>
                                                            </ItemTemplate>
                                                            <FooterTemplate>
                                                                </tbody>
                                                                </table>
                                                           
                                                            </FooterTemplate>
                                                        </asp:Repeater>
                                                    </div>
                                                </ItemTemplate>
                                                <FooterTemplate>
                                                    </tbody>
                                                         </table>
                                                </FooterTemplate>
                                            </asp:Repeater>

                                        </div>
                                    <!-- Order History Ends -->

                                </div>

                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </div>

        </div>
    </section>

</asp:Content>
