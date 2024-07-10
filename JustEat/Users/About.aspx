<%@ Page Title="" Language="C#" MasterPageFile="~/Users/UserMaster.Master" AutoEventWireup="true" CodeBehind="About.aspx.cs" Inherits="JustEat.Users.About" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <!-- about section -->

  <section class="about_section layout_padding">
    <div class="container  ">

      <div class="row">
        <div class="col-md-6 ">
          <div class="img-box">
            <img src="../TemplateFiles/images/about-img.png" alt="">
          </div>
        </div>
        <div class="col-md-6">
          <div class="detail-box">
            <div class="heading_container">
              <h2>
                We Are JustEat
              </h2>
            </div>
            <p>
                We are JustEat—a platform dedicated to bringing you the best in dining options. Our goal is to ensure your experience is seamless and delightful, from order to enjoyment.
                <br />
                <br />
                Your go-to place for tasty meals. Our goal is to make ordering and enjoying food easy and enjoyable. 
                With a diverse menu personalized for your tastes, whether you're in the mood for familiar favorites or want to 
                try something new, we're here to ensure every meal is a delightful experience.
            </p>
            <a href="Menu.aspx">
              Satisfy Your Craving
            </a>
          </div>
        </div>
      </div>
    </div>
  </section>

  <!-- end about section -->

</asp:Content>
