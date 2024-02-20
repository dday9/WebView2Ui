const dispatchMessageReceivedEvent = (request, response) => {
    const messageReceivedEvent = new CustomEvent('messageReceived', {
        detail: { request, response },
        bubbles: true,
        cancelable: true
    });
    window.dispatchEvent(messageReceivedEvent);
};

const buildWebRequest = (controller, route, data) => {
    return {
        Controller: controller,
        Route: route,
        Data: data
    };
};

const toggleLoader = isVisible => {
    let overlay = document.querySelector('.overlay-loading');
    if (isVisible) {
        if (!overlay) {
            overlay = document.createElement('div');
            overlay.classList.add('position-fixed', 'top-0', 'start-0', 'vw-100', 'vh-100', 'd-flex', 'justify-content-center', 'align-items-center', 'overlay-loading', 'opacity-50');
            overlay.style.zIndex = '9999';

            const image = document.createElement('img');
            image.setAttribute('src', 'https://app-assets.local/Images/loading.gif');
            image.classList.add('img-fluid', 'rounded-circle');
            overlay.appendChild(image);
            document.body.appendChild(overlay);
        }
        if (overlay.classList.contains('d-none')) {
            overlay.classList.remove('d-none');
        }
    } else if (overlay && !overlay.classList.contains('d-none')) {
        overlay.classList.add('d-none');
    }
};
