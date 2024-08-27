export enum NotifyLevel {
    Идеальное = 0,
    Хорошее = 1,
    Нормальное = 2,
    Плохое = 3,
    Чрезвычайное = 4,
}

export class LevelToNotify {
    id: number;
    value: string;

    constructor(id: number, value: string) {
        this.id = id;
        this.value = value;
    }
}

export function GetNotifyLevels(): LevelToNotify[] {
    let result = new Array<LevelToNotify>();

    for (const key in NotifyLevel){
        if (!isNaN(Number(NotifyLevel[key]))) {
            console.log("key - ", key);
        result.push(new LevelToNotify(Number(NotifyLevel[key]), key));
        }
        
    }

    return result;
}
