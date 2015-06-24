namespace Devshed.Web
{
    using System.Web.UI;

    /// <summary> Provides validation methods for control validation. </summary>
    public static class ControlExtensions
    {
        /// <summary> Extends controls that have been derrived from the Control class for control validation. 
        /// This will make it possible to validate either WebControls and HtmlControls. </summary>
        /// <param name="control"> The control to validate. </param>
        /// <returns> The created validator. </returns>
        public static ControlValidatorInjector Validation(this Control control)
        {
            return new ControlValidatorInjector(control);
        }
    }
}
