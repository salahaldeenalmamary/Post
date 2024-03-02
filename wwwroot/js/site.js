// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

    // Create a SignalR connection
        const connection = new signalR.HubConnectionBuilder()
        .withUrl("/notificationHub")  // Use the correct URL for your hub
        .configureLogging(signalR.LogLevel.Information)
        .build();

        // Start the connection
        connection.start()
        .then(() => console.log('SignalR Connected'))
        .catch(err => console.error('SignalR Connection Error: ', err));

    // Register the ReceiveNotification method
    connection.on("ReceiveNotification", (message) => {
            // Display the received message in an alert
            alert(message);
    });
  


onPostform = form => {
    $.ajax({
        console.log("kkkkkkkkkkkkkk");
        url: form.action,
        type: 'POST',
        processData: false,
        contentType: false,
        data: new FormData(form),
        success: function (result) {
            console.log(result);
            $("#myModal").modal("hide");
        },
        error: function (error) {
            // Handle error
            console.log(error);
        }
    });
}


    