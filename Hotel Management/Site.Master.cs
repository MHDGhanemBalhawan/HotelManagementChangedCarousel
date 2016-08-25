using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Hotel_Management
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            ////Hide the panel and the calender
            //pnlCheckIn.Visible = false;
        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut();
        }

        protected void calCheckIn_SelectionChanged(object sender, EventArgs e)
        {
            txtCheckInDate.Text = calCheckIn.SelectedDate.ToShortDateString();
            
        }

        protected void calCheckIn_DayRender(object sender, DayRenderEventArgs e)
        {
            if (e.Day.Date < System.DateTime.Today)
            {
                // Disable previous date
                e.Day.IsSelectable = false;
                // Change color of disabled date
                e.Cell.ForeColor = Color.DarkGray;

            }


        }

        protected void calCheckIn_PreRender(object sender, EventArgs e)
        {
            // Disable previous month selection 
            if (calCheckIn.VisibleDate <= System.DateTime.Today)
            {
                calCheckIn.PrevMonthText = string.Empty;

            }
            else
            {
                calCheckIn.PrevMonthText = "&lt;";
            }
        }

        protected void btnCheckIn_Click(object sender, EventArgs e)
        {
            // show the calender if not visiable
            if(pnlCheckIn.Visible==false)
            {
                pnlCheckIn.Visible = true;
            }
            else
            {
                pnlCheckIn.Visible = false;
            }
               
         
        }

        protected void btnCheckOut_Click(object sender, EventArgs e)
        {
            // show the calender if not visiable
            if (pnlCheckOut.Visible == false)
            {
                pnlCheckOut.Visible = true;
            }
            else
            {
                pnlCheckOut.Visible = false;
            }
               
        }

        protected void calCheckOut_PreRender(object sender, EventArgs e)
        {
            // Disable previous month selection 
            if (calCheckOut.VisibleDate <= System.DateTime.Today)
            {
                calCheckOut.PrevMonthText = string.Empty;

            }
            else
            {
                calCheckOut.PrevMonthText = "&lt;";
            }
        }

        protected void calCheckOut_SelectionChanged(object sender, EventArgs e)
        {
            txtCheckOutDate.Text = calCheckOut.SelectedDate.ToShortDateString();
        }

        protected void calCheckOut_DayRender(object sender, DayRenderEventArgs e)
        {
            if (e.Day.Date < System.DateTime.Today)
            {
                // Disable previous date
                e.Day.IsSelectable = false;
                // Change color of disabled date
                e.Cell.ForeColor = Color.DarkGray;

            }
        }

 


    }

}