window.progressBar = {
    start: function () {
        let progressBar = document.getElementById("progress-bar");
        if (!progressBar) return;

        progressBar.style.width = "0%";
        progressBar.style.display = "block";

        let progress = 0;
        let interval = setInterval(() => {
            progress += Math.random() * 50;
            progressBar.style.width = progress + "%";

            if (progress >= 100) {
                window.progressBar.complete();
            } 
        }, 100);

        window.progressBar.interval = interval;
    },
    complete: function () {
        let progressBar = document.getElementById("progress-bar");
        if (!progressBar) return;

        clearInterval(window.progressBar.interval);
        progressBar.style.width = "100%";

        setTimeout(() => {
            progressBar.style.display = "none";
        }, 400);
    }
};

window.chatConnection = {
    connection: null,
    start: async function (dotNetObjRef, userId) {
        this.connection = new signalR.HubConnectionBuilder()
            .withUrl("https://localhost:7257/settings?user=" + encodeURIComponent(userId))
            //.withUrl("https://cp.siddiqueenterprise.com/settings?user=" + encodeURIComponent(userId))
            .build();

        //this.connection.on("ReceiveMessage", function (message) {
        //    dotNetObjRef.invokeMethodAsync("ReceiveMessage",message);
        //});

        await this.connection.start();
    },
    sendMessage: async function (user,message) {
        if (this.connection) {
            this.connection.send("SendSettings", user, message)
                .catch(err => {
                    console.error("Error sending message:", err);
                });
        }
    }
};
