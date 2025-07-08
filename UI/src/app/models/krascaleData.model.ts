export class KRAScaleData
{
    ScaleID :number;
    ScaleLevel?:string;
    MinimumScale:number;
    MaximumScale?:number;
}

export class KRAScaleMaster extends KRAScaleData 
{
  ScaleTitle: string;
  ScaleDetails: KRAScaleDetails[];
}

export class KRAScaleDetails 
{
  ScaleDetailId?: number;
  ScaleValue?: number;
  ScaleDescription?: string;
}