const fs = require('fs');

function imageToBinary(filePath) {
    fs.readFile(filePath, (err, data) => {
        if (err) throw err;

        const binaryData = new Uint8Array(data);

        // Convert to a binary string for visualization
        let binaryString = '';
        binaryData.forEach(byte => {
            // Convert each byte to its binary representation and pad with leading zeros if necessary
            binaryString += byte.toString(2).padStart(8, '0') + ' ';
        });

    });
}


const { chromium } = require('playwright');

(async () => {
    const browser = await chromium.launch({ headless: false });
    const context = await browser.newContext();
    const page = await context.newPage();

    await page.goto('http://localhost:8000');

    await page.waitForSelector('canvas');

    await page.waitForTimeout(10000);

    const canvas = await page.$('canvas');

    await page.keyboard.down('KeyD'); 
    await page.keyboard.down('KeyD'); 
    await page.keyboard.down('KeyD'); 
    await page.keyboard.down('KeyD'); 
    await page.keyboard.down('KeyD'); 
    await page.keyboard.down('KeyD'); 
    const image1 = await page.screenshot({ path: 'screenshot1.png' });
    await page.keyboard.down('KeyD'); 
    await page.keyboard.down('KeyW'); 
    await page.keyboard.down('KeyW'); 
    await page.keyboard.down('KeyW'); 

    await page.keyboard.down('KeyD'); 
    await page.keyboard.down('KeyD'); 


    await page.waitForTimeout(10000); 

    const image2 = await page.screenshot({ path: 'screenshot2.png' });

    // get the image as binary
    const image1Binary = await imageToBinary('screenshot1.png');
    const image2Binary = await imageToBinary('screenshot2.png');

    // Compare the two images
    console.log(image1Binary === image2Binary);

    await browser.close();
})();
