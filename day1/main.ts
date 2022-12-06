import * as fs from 'fs';
import { join } from 'path';

const fileName: string = 'input.txt'

let fileContent = fs.readFileSync(join(__dirname, fileName), 'utf8');
let list = fileContent.split('\n');
let valueList:Array<number> = [];
let currentTotal = 0;
let index = 0;
for (var i = 0; i < list.length ; i++) {
    if (!list.at(i)) {
        valueList[index++] = currentTotal;
        currentTotal = 0;
    }
    let value = +list.at(i);
    currentTotal += value;
}

const sorted:number[] = valueList.sort((a, b) => a - b)

console.log(sorted[sorted.length - 3])
console.log(sorted[sorted.length - 2])
console.log(sorted[sorted.length - 1])

console.log(sorted[sorted.length - 3] + sorted[sorted.length - 2] + sorted[sorted.length - 1])
