﻿using System;
using Windows.Networking.Connectivity;

namespace ATG_Notifier.Desktop.Services
{
    // TODO: 
    //      - Re-enable network checks (metered connection, etc...) once we are working on the Windows 10 version.
    //internal class NetworkService
    //{
    //    public NetworkService()
    //    {
    //        NetworkInformation.NetworkStatusChanged += OnNetworkStatusChanged;

    //        UpdateNetworkProfile();
    //    }

    //    ~NetworkService()
    //    {
    //        NetworkInformation.NetworkStatusChanged -= OnNetworkStatusChanged;
    //    }

    //    public event EventHandler? NetworkProfileChanged;

    //    public bool IsInternetAvailable { get; private set; }

    //    public bool IsMeteredConnection { get; private set; }

    //    private void OnNetworkStatusChanged(object sender)
    //    {
    //        UpdateNetworkProfile();
    //    }

    //    private void UpdateNetworkProfile()
    //    {
    //        var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
    //        if (connectionProfile == null)
    //        {
    //            IsInternetAvailable = false;
    //            NetworkProfileChanged?.Invoke(this, EventArgs.Empty);
    //            return;
    //        }

    //        IsInternetAvailable = true;

    //        if (connectionProfile.GetConnectionCost().NetworkCostType != NetworkCostType.Unrestricted)
    //        {
    //            IsMeteredConnection = true;
    //        }
    //        else
    //        {
    //            IsMeteredConnection = false;
    //        }

    //        NetworkProfileChanged?.Invoke(this, EventArgs.Empty);
    //    }
    //}
}