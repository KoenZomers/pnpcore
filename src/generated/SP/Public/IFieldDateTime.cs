using System;

namespace PnP.Core.Model.SharePoint
{
    /// <summary>
    /// Public interface to define a FieldDateTime object
    /// </summary>
    [ConcreteType(typeof(FieldDateTime))]
    public interface IFieldDateTime : IDataModel<IFieldDateTime>, IDataModelGet<IFieldDateTime>, IDataModelUpdate, IDataModelDelete
    {

        #region New properties

        /// <summary>
        /// To update...
        /// </summary>
        public int DateTimeCalendarType { get; set; }

        /// <summary>
        /// To update...
        /// </summary>
        public string DateFormat { get; set; }

        /// <summary>
        /// To update...
        /// </summary>
        public int DisplayFormat { get; set; }

        /// <summary>
        /// To update...
        /// </summary>
        public int FriendlyDisplayFormat { get; set; }

        /// <summary>
        /// To update...
        /// </summary>
        public string TimeFormat { get; set; }

        #endregion

    }
}
