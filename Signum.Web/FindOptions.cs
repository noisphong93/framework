﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Signum.Entities.DynamicQuery;

namespace Signum.Web
{
    public class FindOptions
    {
        public FindOptions() { }

        public object QueryName { get; set; }

        public bool SearchOnLoad { get; set; }
        
        public FindOptions(object queryName)
        {
            this.QueryName = queryName;
        }

        private SearchButtons buttons = SearchButtons.OkCancel;
        public SearchButtons Buttons
        {
            get { return this.buttons; }
            set { this.buttons = value; }
        }

        private bool? modal;
        public bool Modal
        {
            get { return modal ?? this.Buttons == SearchButtons.OkCancel; }
            set { this.modal = new bool?(value); }
        }

        List<FilterOptions> filterOptions = new List<FilterOptions>();
        public List<FilterOptions> FilterOptions
        {
            get { return filterOptions; }
            set { this.filterOptions = value; }
        }

        public bool AllowMultiple { get; set; }
        
        FilterMode filterMode = FilterMode.Visible;
        public FilterMode FilterMode
        {
            get { return filterMode; }
            set { this.filterMode = value; }
        }
    }

    public class FilterOptions
    {
        public Column Column { get; set; }
        public string ColumnName { get; set; }
        public bool Frozen { get; set; }
        public FilterOperation Operation { get; set; }
        public object Value { get; set; }

        public Filter ToFilter()
        {
            return new Filter
            {
                Column = Column,
                Operation = Operation,
                Value = Value 
            };
        }
    }

    public enum FilterMode
    {
        Visible,
        VisibleAndReadOnly,
        Hidden,
        AlwaysHidden,
    }

    public enum SearchButtons
    {
        OkCancel,
        Close
    }
}
