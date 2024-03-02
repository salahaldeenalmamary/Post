import Swal from 'sweetalert2';

const sweetAlertExtension = {
    showSuccessNotification: (message) => {
        Swal.fire({
            icon: 'success',
            title: 'Success!',
            text: message,
        });
    },
    showErrorNotification: (message) => {
        Swal.fire({
            icon: 'error',
            title: 'Error!',
            text: message,
        });
    },
};

export default sweetAlertExtension;
