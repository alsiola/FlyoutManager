namespace Bulldog.FlyoutManager
{
    /// <summary>
    /// Represents the ongoing direction of change of a Flyout
    /// </summary>
    public class FlyoutAction
    {
        private static string opening = "Opening";
        private static string closing = "Closing";

        private string thisAction;

        protected FlyoutAction( string action )
        {
            thisAction = action;
        }

        public static FlyoutAction Opening
        {
            get { return new FlyoutAction( opening ); }
        }

        public static FlyoutAction Closing
        {
            get { return new FlyoutAction( closing ); }
        }

        public string Action
        {
            get { return thisAction; }
        }

        public override bool Equals( object obj )
        {
            FlyoutAction other = obj as FlyoutAction;
            if( other == null )
                return false;
            return this.Action == other.Action;
        }

        public override int GetHashCode()
        {
            return thisAction.GetHashCode();
        }

        public override string ToString()
        {
            return thisAction;
        }
    }
}
