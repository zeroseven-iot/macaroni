import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { ReconnectingWebSocket } from '../../app/helpers/reconnectingwebsocket';

@Component({
	selector: 'page-home',
	templateUrl: 'home.html'
})
export class HomePage {
	public azureSocket:ReconnectingWebSocket;
	constructor(public navCtrl: NavController) {
		//this.azureSocket = new ReconnectingWebSocket("wss://<YOUR-IOT-HUB-NAME>.azure-devices.net:443/$iothub/websocket", ["AMQPWSB10"]);
		this.azureSocket = new ReconnectingWebSocket("ws://echo.websocket.org")
	}

	public sendMessageToAzure() {
		this.azureSocket.send("This is a test");
	}
}
