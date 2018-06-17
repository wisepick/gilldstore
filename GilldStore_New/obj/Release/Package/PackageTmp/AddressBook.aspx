<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AddressBook.aspx.cs" Inherits="GilldStore_New.AddressBook" %>

<%@ Register Src="~/Controls/Address_Book.ascx" TagName="AddressBook" TagPrefix="uc1" %>
<html lang="en" ng-app="storeApp">
    <head runat="server">        
        <meta name="google-site-verification" content="CHu08yoD1DocIsHKSAtts6vxjvytnupTpaMfPHLI5Y8" />
        <meta charset="utf-8">
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <meta name="viewport" content="width=device-width, initial-scale=1">
        <title><%= Application["COMPANY_NAME"]%></title>
        <meta name="Title" content="<%= Application["COMPANY_NAME"].ToString() + " - " + Application["META_TITLE"].ToString() %>" />
        <meta name="description" content="<%= Application["COMPANY_NAME"].ToString() + " - " + Application["META_DESCRIPTION"].ToString() %>" />
        <meta name="keywords" content="<%= Application["META_KEYWORDS"].ToString() %>" />
    
        <!-- PLUGINS CSS STYLE -->
        <link href="~/plugins/jquery-ui/jquery-ui.css" rel="tsylesheet" runat="server">
        <link href="~/plugins/bootstrap/css/bootstrap.min.css" rel="stylesheet" runat="server">
        <link href="~/plugins/font-awesome/css/font-awesome.min.css" rel="stylesheet" runat="server">
        <link rel="stylesheet" type="text/css" href="~/plugins/rs-plugin/css/settings.css" media="screen" runat="server">
        <link rel="stylesheet" type="text/css" href="~/plugins/selectbox/select_option1.css" runat="server">
        <link rel="stylesheet" type="text/css" href="~/plugins/owl-carousel/owl.carousel.css" media="screen" runat="server">
        <link rel="stylesheet" type="text/css" href="~/plugins/isotope/jquery.fancybox.css" runat="server">
        <link rel="stylesheet" type="text/css" href="~/plugins/isotope/isotope.css" runat="server">

        <!-- GOOGLE FONT -->
        <link href='https://fonts.googleapis.com/css?family=Open+Sans:400,300,600,700' rel='stylesheet' type='text/css'>
        <link href='https://fonts.googleapis.com/css?family=Dosis:400,300,600,700' rel='stylesheet' type='text/css'>

        <!-- CUSTOM CSS -->
        <link href="~/css/style.css" rel="stylesheet" runat="server">
        <link rel="stylesheet" href="~/css/default.css" id="option_color" runat="server"> 

        <!-- Icons -->
        <link rel="shortcut icon" href="~/img/logo.png" runat="server">

        <!-- HTML5 shim and Respond.js for IE8 support of HTML5 elements and media queries -->
        <!-- WARNING: Respond.js doesn't work if you view the page via file:// -->
        <!--[if lt IE 9]>
            <script src="https://oss.maxcdn.com/html5shiv/3.7.2/html5shiv.min.js"></script>
            <script src="https://oss.maxcdn.com/respond/1.4.2/respond.min.js"></script>
        <![endif]-->        
    </head>
    <body>
<form id="form1" runat="server">
    <uc1:AddressBook 
                            ID="AddressBook1" 
                            runat="server"                 
                            Display_DeliveryAddressOption="N"
                        />
    <div class="col-sm-1"

</form></body>