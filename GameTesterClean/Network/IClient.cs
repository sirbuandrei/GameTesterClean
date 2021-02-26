﻿using System;
using System.Collections.Generic;
using GPNetworkMessage;

/*
 * Written by Tim Stoddard
 * Multiplayer Games & Software Engineering
 * Staffordshire University
 */

namespace GPNetworkClient
{
    public interface IClient
    {
        int ClientID { get; }
        int ClientAmount { get; }
        bool IsConnected { get; }
        Queue<AMessage> Messages { get; }
        bool Connect(string hostname, int port, string message = "Client has connected");
        void Disconnect(string message = "Client Disconnected");
        AMessage ReceiveMessage();
        void MessageReceiver();
        void SendMessage(GPNetworkMessage.MessageType type, string data);
        void SendMessageExceptOne(string data, int ClientID);
        void SendMessageToOne(string data, int ClientID);
    }
}

/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2015 Tim Stoddard
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 * 
 */
