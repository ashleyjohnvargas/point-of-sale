const allSideMenu = document.querySelectorAll('#sidebar .side-menu.top li a');

allSideMenu.forEach(item => {
	const li = item.parentElement;

	item.addEventListener('click', function () {
		allSideMenu.forEach(i => {
			i.parentElement.classList.remove('active');
		})
		li.classList.add('active');
	})
});




// TOGGLE SIDEBAR
const menuBar = document.querySelector('#content nav .bx.bx-menu');
const sidebar = document.getElementById('sidebar');

menuBar.addEventListener('click', function () {
	sidebar.classList.toggle('hide');
})

const searchButton = document.querySelector('#content nav form .form-input button');
const searchButtonIcon = document.querySelector('#content nav form .form-input button .bx');
const searchForm = document.querySelector('#content nav form');

searchButton.addEventListener('click', function (e) {
	if (window.innerWidth < 576) {
		e.preventDefault();
		searchForm.classList.toggle('show');
		if (searchForm.classList.contains('show')) {
			searchButtonIcon.classList.replace('bx-search', 'bx-x');
		} else {
			searchButtonIcon.classList.replace('bx-x', 'bx-search');
		}
	}
})

if (window.innerWidth < 768) {
	sidebar.classList.add('hide');
} else if (window.innerWidth > 576) {
	searchButtonIcon.classList.replace('bx-x', 'bx-search');
	searchForm.classList.remove('show');
}

window.addEventListener('resize', function () {
	if (this.innerWidth > 576) {
		searchButtonIcon.classList.replace('bx-x', 'bx-search');
		searchForm.classList.remove('show');
	}
})

// Dark Mode Toggle
const switchMode = document.getElementById('switch-mode');

// Apply dark mode state on page load
if (localStorage.getItem('dark-mode') === 'enabled') {
	document.body.classList.add('dark');
	switchMode.checked = true;
}

// Toggle dark mode and save state
switchMode.addEventListener('change', function () {
	if (this.checked) {
		document.body.classList.add('dark');
		localStorage.setItem('dark-mode', 'enabled');
	} else {
		document.body.classList.remove('dark');
		localStorage.setItem('dark-mode', 'disabled');
	}
});


// Fullscreen Toggle
const fullscreenToggle = document.getElementById('fullscreen-toggle');
const fullscreenIcon = document.getElementById('fullscreen-icon');
const exitFullscreenIcon = document.getElementById('exit-fullscreen-icon');

// Apply fullscreen state on page load
if (localStorage.getItem('fullscreen') === 'enabled') {
	document.documentElement.requestFullscreen().catch(console.error);
	document.getElementById('fullscreen-icon').style.display = 'none';
	document.getElementById('exit-fullscreen-icon').style.display = 'inline';
}

// Toggle fullscreen mode and save state
document.getElementById('fullscreen-toggle').addEventListener('click', function (event) {
	event.preventDefault();

	const fullscreenIcon = document.getElementById('fullscreen-icon');
	const exitFullscreenIcon = document.getElementById('exit-fullscreen-icon');

	if (!document.fullscreenElement) {
		document.documentElement.requestFullscreen().then(() => {
			localStorage.setItem('fullscreen', 'enabled');
			fullscreenIcon.style.display = 'none';
			exitFullscreenIcon.style.display = 'inline';
		}).catch(err => console.error(`Error enabling fullscreen: ${err.message}`));
	} else {
		document.exitFullscreen().then(() => {
			localStorage.setItem('fullscreen', 'disabled');
			fullscreenIcon.style.display = 'inline';
			exitFullscreenIcon.style.display = 'none';
		}).catch(err => console.error(`Error exiting fullscreen: ${err.message}`));
	}
});



// Logout Confirmation
document.querySelector('.logout').addEventListener('click', function (event) {
	event.preventDefault();
	const userConfirmed = confirm("Are you sure you want to logout?");
	if (userConfirmed) {
		window.location.href = '/logout'; // Redirect to logout route
	}
});


/*
<script>
	function confirmLogout(event) {
	  event.preventDefault(); // Prevent default link behavior
	  const userConfirmed = confirm("Are you sure you want to logout?");
	  if (userConfirmed) {
		// Redirect to the logout endpoint
		window.location.href = '/logout'; // Update this URL to match your logout route
	  }
	}
</script> */
// Function to increase quantity
// Initialize subtotal values on page load
function initializeCart() {
	let cartRows = document.querySelectorAll("[id^='row-']");
	cartRows.forEach((row) => {
		let productId = row.id.replace('row-', '');
		updateSubtotal(productId); // Set initial subtotal for each product
	});
}

// Increase quantity
function increaseQuantity(productId) {
	let quantityElement = document.getElementById(`quantity-${productId}`);
	let currentQuantity = parseInt(quantityElement.textContent);
	quantityElement.textContent = currentQuantity + 1;
	updateSubtotal(productId);
}

// Decrease quantity
function decreaseQuantity(productId) {
	let quantityElement = document.getElementById(`quantity-${productId}`);
	let currentQuantity = parseInt(quantityElement.textContent);
	if (currentQuantity > 1) {
		quantityElement.textContent = currentQuantity - 1;
		updateSubtotal(productId);
	}
}

// Update subtotal for a specific product
function updateSubtotal(productId) {
	let quantity = parseInt(document.getElementById(`quantity-${productId}`).textContent);
	let price = parseFloat(document.getElementById(`price-${productId}`).textContent);
	let subtotalElement = document.getElementById(`subtotal-${productId}`);
	subtotalElement.textContent = `₱${(quantity * price).toFixed(2)}`;
	updateTotal();
}

// Update total amount
function updateTotal() {
	let subtotals = document.querySelectorAll("[id^='subtotal-']");
	let total = 0;
	subtotals.forEach((element) => {
		total += parseFloat(element.textContent.replace('₱', ''));
	});
	document.getElementById("total-amount").textContent = `₱${total.toFixed(2)}`;
}

// Remove item
function removeItem(productId) {
	let row = document.getElementById(`row-${productId}`);
	if (row) {
		row.remove();
		updateTotal();
	} else {
		console.error(`Row with ID 'row-${productId}' not found.`);
	}
}

// Mock checkout function
function checkout() {
	alert("Checkout complete!");
}

// Initialize the cart on page load
initializeCart();



function updateStatus(element) {
	const newValue = element.value;
	const invoiceNumber = element.closest('tr').querySelector('td:first-child').innerText;

	console.log(`Invoice No. ${invoiceNumber}: Status updated to ${newValue}`);

	// Optional: Make an AJAX call to update the status in your database
	// Example:
	fetch('/updateStatus', {
		method: 'POST',
		headers: {
			'Content-Type': 'application/json',
		},
		body: JSON.stringify({ invoiceNumber, newValue }),
	}).then(response => {
		if (response.ok) {
			alert('Status updated successfully!');
		} else {
			alert('Failed to update status');
		}
	});
}