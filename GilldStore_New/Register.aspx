﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="GilldStore_New.Register" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div id="container">
        <div class="row">
            <br />
            <div class="text-center">
                <a runat="server" href="~/Account/Login.aspx">Allready have an account? Log in</a>
            </div>
            <div id="root" style="width: 320px; margin: 40px auto; padding: 10px; border-style: dashed; border-width: 1px; box-sizing: border-box;">
                embedded area
            </div>
            <script src="https://cdn.auth0.com/js/lock/10.16/lock.min.js"></script>
            <script>
                var lock = new Auth0Lock('<%= ConfigurationManager.AppSettings["auth0:ClientId"].ToString() %>', '<%= ConfigurationManager.AppSettings["auth0:Domain"].ToString() %>', {
                           theme: {
                                logo: 'http://<%= Request.ServerVariables["HTTP_HOST"] %>/img/<%= Application["COMPANY_LOGO"].ToString() %>'            
                           },
                           languageDictionary: {
                                title: 'Sign Up'
                           },
                           allowLogin: false,
                           socialBigButtons : true,
                           container: 'root',
                           auth: {<%if (GilldStore_New.App_Start.CommonClass.Is_Production()) {%>redirectUrl: 'https://<%= Request.ServerVariables["HTTP_HOST"] %>/LoginCallback.ashx?r_url=<%= Request.QueryString["r_url"] %>'
                                  <%} else {%>
                                        redirectUrl: 'http://<%= Request.ServerVariables["HTTP_HOST"] %>/LoginCallback.ashx?r_url=<%= Request.QueryString["r_url"] %>'
                                  <%}%>,
                            responseType: 'code',
                            params: {
                                scope: 'openid email' // Learn about scopes: https://auth0.com/docs/scopes
                            }
                            }
                        });
                lock.show();
            </script>
        </div>
    </div>   
</asp:Content>
