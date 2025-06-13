using System;
using ReactiveUI;

namespace Avalonia.UIStudio.Driver.ViewModels
{
    public class ViewModelBase : ReactiveObject, IDisposable
    {

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                
            }
        }

        /// <summary>
        /// Releases all resources used by the <see cref="ViewModelBase"/> instance.
        /// </summary>
        /// <remarks>
        /// This method calls the <see cref="Dispose(bool)"/> method with a value of <c>true</c> 
        /// to release managed resources and suppresses finalization of the object.
        /// </remarks>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
