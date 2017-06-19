import { Component } from '@angular/core';
import { NavController } from 'ionic-angular';
import { ReconnectingWebSocket } from '../../app/helpers/reconnectingwebsocket';

@Component({
	selector: 'page-home',
	templateUrl: 'home.html'
})
export class HomePage {
	public azureSocket:ReconnectingWebSocket;

	private messageStatus:string = "";

	constructor(public navCtrl: NavController) {
		//this.azureSocket = new ReconnectingWebSocket("wss://<YOUR-IOT-HUB-NAME>.azure-devices.net:443/$iothub/websocket", ["AMQPWSB10"]);
		this.azureSocket = new ReconnectingWebSocket("ws://echo.websocket.org")
	}

	public sendMessageToAzure() {
		this.azureSocket.send("This is a test");
	}

	sendMessageToEventHub() {

		var self = this;
		// Generate a SAS key with the Signature Generator.: https://github.com/sandrinodimattia/RedDog/releases
		// Could be provided by a Web API.
		var sas = "SharedAccessSignature sr=https%3a%2f%2freddogeventhub.servicebus.windows.net%2fmesssage%2fpublishers%2fphone%2fmessages&sig=Luqu%2fZQd6rfhCdGPTZlhMYCVtXM51QWsdSVlc08LGWc%3d&se=1405564221&skn=SenderDevice";

		var serviceNamespace = "customeventhub";
		var hubName = "iotHub";
		var deviceName = "phone";

		var xmlHttpRequest = new XMLHttpRequest();

		xmlHttpRequest.open("POST", "https://" + serviceNamespace + ".servicebus.windows.net/" + hubName + "/publishers/" + deviceName + "/messages", true);
		xmlHttpRequest.setRequestHeader('Content-Type', "application/atom+xml;type=entry;charset=utf-8");
		xmlHttpRequest.setRequestHeader("Authorization", sas);

		xmlHttpRequest.onreadystatechange = function () {
			if (this.readyState == 4) {

				if (this.status == 201) {
					self.messageStatus = "Status: Ready to send";
				} else {
					self.messageStatus = "Status: " + this.status.toString();
				}
			}
		};
    	xmlHttpRequest.send("{ Message: Test message }");
	}
}
