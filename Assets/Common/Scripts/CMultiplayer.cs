/*
 * Copyright (C) 2014 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using UnityEngine;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi.Multiplayer;
using System.Collections.Generic;
using System;

public class CMultiplayer : MonoBehaviour, RealTimeMultiplayerListener
{
    const int QuickGameOpponents = 1;
    const int GameVariant = 0;
    const int MinOpponents = 1;
    const int MaxOpponents = 3;

    static CMultiplayer instance = null;

    private void Awake()
    {
        instance = this;
    }

    public void CreateQuickGame()
    {
        PlayGamesPlatform.Instance.RealTime.CreateQuickGame(QuickGameOpponents, QuickGameOpponents,
                GameVariant, instance);
    }

    public void OnRoomConnected(bool success)
    {
        if (success)
        {

        }
        else
        {

        }
    }

    public void OnLeftRoom()
    {

    }

    public void OnPeersConnected(string[] peers)
    {
    }

    public void OnPeersDisconnected(string[] peers)
    {

    }

    public void OnRoomSetupProgress(float percent)
    {

    }

    public void OnRealTimeMessageReceived(bool isReliable, string senderId, byte[] data)
    {

    }

    public void CleanUp()
    {
        PlayGamesPlatform.Instance.RealTime.LeaveRoom();

        instance = null;
    }

    private Participant GetSelf()
    {
        return PlayGamesPlatform.Instance.RealTime.GetSelf();
    }

    private List<Participant> GetPlayers()
    {
        return PlayGamesPlatform.Instance.RealTime.GetConnectedParticipants();
    }

    private Participant GetParticipant(string participantId)
    {
        return PlayGamesPlatform.Instance.RealTime.GetParticipant(participantId);
    }
}