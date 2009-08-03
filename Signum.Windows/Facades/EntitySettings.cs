﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using System.ComponentModel;
using System.Windows.Media;
using System.Windows.Data;
using System.Collections;
using Signum.Utilities.DataStructures;
using Signum.Entities;

namespace Signum.Windows
{
    public class EntitySettings
    {
        public Func<Control> View { get; set; }
        public Func<Window> ViewWindow { get; set; }

        public Func<bool, bool> IsCreable { get; set; }
        public Func<bool, bool> IsReadOnly { get; set; }
        public Func<bool, bool> IsViewable { get; set; }
        public Func<bool, bool> ShowOkSave { get; set; }

        public Action<bool, ICollectionView> CollectionViewOperations { get; set; }
        public DataTemplate DataTemplate { get; set; }
        public ImageSource Icon { get; set; }

        public EntitySettings()
            : this(EntityType.Default)
        {
        }

        public EntitySettings(EntityType entityType)
        {
            switch (entityType)
            {
                case EntityType.Default:
                    break;
                case EntityType.Admin:
                    IsReadOnly = admin => !admin;
                    IsCreable = admin => admin;
                    IsViewable = admin => admin;
                    CollectionViewOperations = (isLazy, cv) =>
                    {
                        ListCollectionView lcv = cv as ListCollectionView;
                        if (lcv != null)
                            lcv.CustomSort = isLazy ?
                                (IComparer)new LambdaComparer<Lazy, int>(la => la.IdOrNull ?? int.MaxValue) :
                                (IComparer)new LambdaComparer<IdentifiableEntity, int>(ie => ie.IdOrNull ?? int.MaxValue);
                        else
                            cv.SortDescriptions.Add(new SortDescription("IdOrNull", ListSortDirection.Ascending));
                    };
                    break;
                case EntityType.NotSaving:
                    ShowOkSave = admin => false; 
                    break;
                case EntityType.ServerOnly:
                    IsReadOnly = admin => true;
                    ShowOkSave = admin => false; 
                    IsCreable = admin => false;
                    break;
                default:
                    break;
            }
        }
    }

    public enum WindowsType
    {
        View,
        Find,
        Admin
    }

    public enum EntityType
    {
        Admin,
        Default,
        NotSaving,
        ServerOnly,
    }
}
