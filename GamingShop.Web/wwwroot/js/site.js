// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const btn = document.querySelector(".hamburger-menu");
const menu = document.querySelector(".nav-links");
const container = document.querySelector(".nav-links-container");
const showSubMenuBtn = document.querySelector(".showSubMenu");
const submenu = document.querySelector(".nav-link-submenu");
const emailInput = document.getElementById("email-text");
const userNameInput = document.getElementById("username-text");
const phoneInput = document.getElementById("phone-text");
const emailbtn = document.getElementById("btn-email");
const usernamebtn = document.getElementById("btn-username");
const phonebtn = document.getElementById("btn-phone");
const saveBtn = document.getElementById("save-btn");

btn.addEventListener("click", () => {
    console.log("works");
    menu.classList.toggle("display");
    container.classList.toggle("display-flex");
    console.log("w");
});

showSubMenuBtn.addEventListener("click", () => {
    submenu.classList.toggle("display-submenu");
});

emailbtn.addEventListener("click", () => {
    emailInput.focus();
    emailInput.scrollIntoView();
    saveBtn.classList.toggle("show-save-btn");
    emailInput.readOnly = !emailInput.readOnly ;
});

usernamebtn.addEventListener("click", () => {
    userNameInput.focus();
    userNameInput.scrollIntoView();
    saveBtn.classList.toggle("show-save-btn");
    userNameInput.readOnly = !userNameInput.readOnly;
});

phonebtn.addEventListener("click", () => {
    phoneInput.focus();
    phoneInput.scrollIntoView();
    saveBtn.classList.toggle("show-save-btn");
    phoneInput.readOnly = !phoneInput.readOnly;
});
