<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="error.aspx.cs" Inherits="GilldStore_New.error" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Gilld Store</title>
    <link href="css/style1.css" rel="stylesheet"/>
    <link rel="stylesheet" href="css/default.css" id="option_color" /> 
    <link href="plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" />
</head>
<body>
    
      <section class="mainContent full-width clearfix">
      <div class="container">
        <div class="row">
          <div class="col-sm-4 col-sm-offset-1 col-xs-12">            
              <a class="navbar-brand" runat="server" href="~/"><img runat="server" src="~/img/logo.png" alt="Gilld Store" width="250"></a>
          </div>
          <div class="col-sm-4 col-sm-offset-1 col-xs-12">            
              <img src="~/img/error.png" runat="server" width="300"/>
          </div>
          <div class="col-sm-6 col-xs-12">
            <div class="errorInfo">             
              <h3>Oops!</h3>
              <p>We are sorry, It's not for you. It's us.</p>
              <p>We're experiencing an internal server problem. Please try again or contact <a href="mailto:admin@gilldstore.in">admin@gilldstore.in</a></p>
              
                <div class="formBtnArea pull-left">
                  <a href="~/" runat="server" class="btn btn-primary bg-color-3">Return to home page</a>
                </div>
              
            </div>
          </div>
        </div>

      </div>
    </section>
</body>
</html>
