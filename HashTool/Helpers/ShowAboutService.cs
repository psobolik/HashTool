using GalaSoft.MvvmLight.Messaging;
using HashTool.Messaging;
using HashTool.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace HashTool.Helpers
{
    public class ShowAboutService
    {
        public ShowAboutService()
        {
        }

        public ShowAboutService(Window owner)
        {
            Messenger.Default.Register<NotificationMessage>(
                this,
                msg =>
                {
                    if (msg.Notification == Notifications.ShowAbout)
                    {
                        AboutView view = new AboutView { Owner = owner, };
                        view.ShowDialog();
                    }
                });
        }
    }
}
