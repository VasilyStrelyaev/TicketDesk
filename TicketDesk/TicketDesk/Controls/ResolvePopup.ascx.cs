﻿// TicketDesk - Attribution notice
// Contributor(s):
//
//      Stephen Redd (stephen@reddnet.net, http://www.reddnet.net)
//
// This file is distributed under the terms of the Microsoft Public 
// License (Ms-PL). See http://www.codeplex.com/TicketDesk/Project/License.aspx
// for the complete terms of use. 
//
// For any distribution that contains code from this file, this notice of 
// attribution must remain intact, and a copy of the license must be 
// provided to the recipient.
using System;
using System.Web.UI;
using TicketDesk.Engine;
using TicketDesk.Engine.Linq;

namespace TicketDesk.Controls
{
    public partial class ResolvePopup : System.Web.UI.UserControl
    {
        protected void Page_PreRender(object sender, EventArgs e)
        {
            ShowResolveButton.Visible = (TicketToDisplay.CurrentStatus != "Closed") &&
                (TicketToDisplay.CurrentStatus != "Resolved") &&
                (TicketToDisplay.AssignedTo == Page.User.Identity.GetFormattedUserName());

        }
        public event TicketPropertyChangedDelegate Resolved;
        private Ticket _ticket;
        public Ticket TicketToDisplay
        {
            get
            {
                return _ticket;
            }
            set
            {
                _ticket = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }

        protected void ResolveButton_Click(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            

            TicketToDisplay.CurrentStatus = "Resolved";
            TicketToDisplay.CurrentStatusDate = now;
            TicketToDisplay.CurrentStatusSetBy = Page.User.Identity.GetFormattedUserName();

            
            TicketComment comment = new TicketComment();
            
            comment.CommentEvent = string.Format("has resolved the ticket");

            comment.CommentedBy = Page.User.Identity.GetFormattedUserName();
            comment.IsHtml = false;
            if(CommentsTextBox.Text.Trim() != string.Empty)
            {
                comment.Comment = CommentsTextBox.Text.Trim();
            }
            
            TicketToDisplay.TicketComments.Add(comment);

            ResolveModalPopupExtender.Hide();
            if(Resolved != null)
            {
                Resolved();
            }
            
        }
    }
}