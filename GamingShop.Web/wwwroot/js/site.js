// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
const btn = document.querySelector(".hamburger-menu");
const menu = document.querySelector(".nav-links");
const container = document.querySelector(".nav-links-container");

const showSubMenuBtn = document.querySelector(".showSubMenu");
const submenu = document.querySelector(".nav-link-submenu");

btn.addEventListener("click", () => {
    menu.classList.toggle("display");
    container.classList.toggle("display-flex");
    console.log("w");
});

showSubMenuBtn.addEventListener("click", () => {
    submenu.classList.toggle("display-submenu");
});

const emailInput = document.getElementById("email-text");
const userNameInput = document.getElementById("username-text");
const phoneInput = document.getElementById("phone-text");

const emailbtn = document.getElementById("btn-email");
const usernamebtn = document.getElementById("btn-username");
const phonebtn = document.getElementById("btn-phone");

const saveBtn = document.getElementById("save-btn");

emailbtn.addEventListener("click", () => {
    emailInput.focus();
    emailInput.scrollIntoView();
    saveBtn.classList.toggle("show-save-btn");
});

usernamebtn.addEventListener("click", () => {
    userNameInput.focus();
    userNameInput.scrollIntoView();
    saveBtn.classList.toggle("show-save-btn");
});

phonebtn.addEventListener("click", () => {
    phoneInput.focus();
    phoneInput.scrollIntoView();
    saveBtn.classList.toggle("show-save-btn");
});
