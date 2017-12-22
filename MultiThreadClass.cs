using System;
using System.Windows.Forms;
using System.Threading;

namespace testgistogr
{
    internal static class MultiThreadClass
    {
        static Thread thread;
        private static void ApplicationRunProc(object state)
        {
            Application.Run(state as Form);
        }

        public static void RunInNewThread(this Form form, bool isBackground)
        {
            if (form == null)
                throw new ArgumentNullException("form");
            if (form.IsHandleCreated)
                throw new InvalidOperationException("Form is already running.");
            thread = new Thread(ApplicationRunProc);
            thread.SetApartmentState(ApartmentState.STA);
            thread.IsBackground = isBackground;
            thread.Start(form);
        }
        public static void StopInNewThread(this Form form)
        {
            thread.Abort();
        }
    }
}
