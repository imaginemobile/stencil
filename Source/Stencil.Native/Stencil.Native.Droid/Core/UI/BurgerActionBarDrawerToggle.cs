﻿using System;
using Android.App;
using Android.Views;
using Android.Support.V7.App;
using Android.Support.V4.Widget;
using Android.Views.Animations;
using Android.OS;

namespace Stencil.Native.Droid.Core.UI
{
    public delegate void ActionBarDrawerChangedEventHandler(object s, ActionBarDrawerEventArgs e);
    public class ActionBarDrawerEventArgs : EventArgs
    {
        public View DrawerView { get; set; }
        public float SlideOffset { get; set; }
        public int NewState { get; set; }
    }

    public class BurgerActionBarDrawerToggle : ActionBarDrawerToggle
    {
        public BurgerActionBarDrawerToggle(Activity activity, DrawerLayout drawerLayout, Android.Support.V7.Widget.Toolbar toolbar, int openDrawerContentDescRes, int closeDrawerContentDescRes)
            : base(activity, drawerLayout, toolbar, openDrawerContentDescRes, closeDrawerContentDescRes)
        { }

        public event ActionBarDrawerChangedEventHandler DrawerClosed;
        public event ActionBarDrawerChangedEventHandler DrawerOpened;
        public event ActionBarDrawerChangedEventHandler DrawerSlide;
        public event ActionBarDrawerChangedEventHandler DrawerStateChanged;


        public override void OnDrawerClosed(View drawerView)
        {
            if (null != this.DrawerClosed)
            {
                this.DrawerClosed(this, new ActionBarDrawerEventArgs { DrawerView = drawerView });
            }
            base.OnDrawerClosed(drawerView);
        }

        public override void OnDrawerOpened(View drawerView)
        {
            if (null != this.DrawerOpened)
            {
                this.DrawerOpened(this, new ActionBarDrawerEventArgs { DrawerView = drawerView });
            }
            base.OnDrawerOpened(drawerView);
        }

        public override void OnDrawerSlide(View drawerView, float slideOffset)
        {
            if (null != this.DrawerSlide)
            {
                this.DrawerSlide(this, new ActionBarDrawerEventArgs
                {
                    DrawerView = drawerView,
                    SlideOffset = slideOffset
                });
            }
            base.OnDrawerSlide(drawerView, slideOffset);

        }

        public override void OnDrawerStateChanged(int newState)
        {
            if (null != this.DrawerStateChanged)
            {
                this.DrawerStateChanged(this, new ActionBarDrawerEventArgs
                {
                    NewState = newState
                });
            }
            base.OnDrawerStateChanged(newState);
        }
    }


}

